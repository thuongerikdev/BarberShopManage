using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Dtos.User;
using BM.Auth.Infrastructure;
using BM.Constant;
using BM.Shared.ApplicationService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Implements
{
    public class AuthUserService : AuthServiceBase, IAuthUserService, AuthDataSend
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabase _redisDb;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IAuthEmpService _authEmpService;
        private readonly IAuthCustomerService _authCustomerService;

        public AuthUserService(
            ILogger<AuthUserService> logger,
            AuthDbContext dbContext,
            IConfiguration configuration,
            IEmailService emailService,
            IConnectionMultiplexer redis,
            ICloudinaryService cloudinaryService,
            IAuthCustomerService authCustomerService,
            IAuthEmpService authEmpService,
            IConfiguration config) : base(logger, dbContext)
        {
            _configuration = configuration;
            _redisDb = redis.GetDatabase();
            _config = config;
            _emailService = emailService;
            _cloudinaryService = cloudinaryService;
            _authEmpService = authEmpService;
            _authCustomerService = authCustomerService;
        }

        private string SecretKey => _configuration["Jwt:SecretKey"];

        private string GenerateToken(string name, int userId, List<AuthPermission> authPermissions)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim("name", name),
                new Claim("userId", userId.ToString())
            };
            foreach (var permission in authPermissions)
            {
                claims.Add(new Claim("permission", permission.permissionName));
            }

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateVerificationToken()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<ResponeDto> AuthRegister(AuthRegisterDto authRegisterDto)
        {
            _logger.LogInformation("Starting AuthRegister process");

            if (authRegisterDto == null)
            {
                return ErrorConst.Error(400, "Dữ liệu đăng ký không hợp lệ.");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(authRegisterDto.userName) ||
                    string.IsNullOrWhiteSpace(authRegisterDto.email) ||
                    string.IsNullOrWhiteSpace(authRegisterDto.password))
                {
                    return ErrorConst.Error(400, "Dữ liệu đầu vào không hợp lệ.");
                }

                // Validate email format
                var emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                if (!Regex.IsMatch(authRegisterDto.email, emailRegex))
                {
                    return ErrorConst.Error(400, "Định dạng email không hợp lệ.");
                }

                // Validate phone number (10-11 digits, starts with 0, optional)
                if (!string.IsNullOrWhiteSpace(authRegisterDto.phoneNumber))
                {
                    var phoneRegex = @"^0\d{9,10}$";
                    if (!Regex.IsMatch(authRegisterDto.phoneNumber, phoneRegex))
                    {
                        return ErrorConst.Error(400, "Số điện thoại không hợp lệ. Phải bắt đầu bằng 0 và có 10-11 chữ số.");
                    }
                }

                // Kiểm tra user tồn tại
                var existingUser = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.userName == authRegisterDto.userName);

                if (existingUser != null)
                {
                    return ErrorConst.Error(409, "Người dùng đã tồn tại.");
                }

                // Kiểm tra email đã được sử dụng
                //var existingEmail = await _dbContext.Users
                //    .AsNoTracking()
                //    .AnyAsync(x => x.email == authRegisterDto.email);

                //if (existingEmail)
                //{
                //    return ErrorConst.Error(409, "Email đã được sử dụng.");
                //}

                // Băm mật khẩu
                var (passwordHash, passwordSalt) = PasswordHasher.HashPassword(authRegisterDto.password);
                var verificationToken = GenerateVerificationToken();

                // Tạo user mới
                var user = new AuthUser
                {
                    userName = authRegisterDto.userName,
                    passwordHash = passwordHash,
                    passwordSalt = passwordSalt,
                    email = authRegisterDto.email,
                    phoneNumber = authRegisterDto.phoneNumber,
                    fullName = authRegisterDto.fullName,
                    avatar = "",
                    dateOfBirth = authRegisterDto.dateOfBirth,
                    gender = authRegisterDto.gender,
                    roleID = authRegisterDto.roleID,
                    token = "",
                    isEmailVerified = false,
                    isEmp = false,
                    emailVerificationToken = verificationToken
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                // Lấy user vừa tạo
                var userRes = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.userName == authRegisterDto.userName);

                if (userRes == null)
                {
                    await transaction.RollbackAsync();
                    return ErrorConst.Error(500, "Không thể tìm thấy người dùng vừa tạo.");
                }

                // Tạo customer
                var customer = new AuthCreateCustomerDto
                {
                    userID = userRes.userID,
                    customerStatus = "OK",
                    customerType = "Normal",
                    vipID = 1
                };

                var customerResult = await _authCustomerService.AuthCreateCustomer(customer);
                if (customerResult.ErrorCode != 200)
                {
                    await transaction.RollbackAsync();
                    return ErrorConst.Error(500, "Tạo khách hàng thất bại.");
                }

                await _dbContext.SaveChangesAsync();

                // Gửi email xác thực
                await _emailService.SendVerificationEmail(userRes.userID, userRes.email, verificationToken);

                await transaction.CommitAsync();
                return ErrorConst.Success("Đăng ký thành công. Vui lòng kiểm tra email để xác nhận.", null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error in AuthRegister: {Message}", ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthRegisterEmp(List<AuthRegisteEmprDto> authRegisterDtos)
        {
            _logger.LogInformation("Starting AuthRegisterEmp process for {Count} users", authRegisterDtos?.Count ?? 0);

            if (authRegisterDtos == null || !authRegisterDtos.Any())
            {
                return ErrorConst.Error(400, "Danh sách thông tin đăng ký không hợp lệ.");
            }

            var results = new List<string>();
            var failedCount = 0;

            foreach (var authRegisterDto in authRegisterDtos)
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // Validate input
                    if (string.IsNullOrWhiteSpace(authRegisterDto.userName) ||
                        string.IsNullOrWhiteSpace(authRegisterDto.email) ||
                        string.IsNullOrWhiteSpace(authRegisterDto.password))
                    {
                        results.Add($"Thất bại: Dữ liệu đầu vào không hợp lệ cho user {authRegisterDto.userName}.");
                        failedCount++;
                        await transaction.RollbackAsync();
                        continue;
                    }

                    // Validate email format
                    var emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                    if (!Regex.IsMatch(authRegisterDto.email, emailRegex))
                    {
                        results.Add($"Thất bại: Định dạng email không hợp lệ cho user {authRegisterDto.userName}.");
                        failedCount++;
                        await transaction.RollbackAsync();
                        continue;
                    }

                    // Validate phone number (10-11 digits, starts with 0, optional)
                    if (!string.IsNullOrWhiteSpace(authRegisterDto.phoneNumber))
                    {
                        var phoneRegex = @"^0\d{9,10}$";
                        if (!Regex.IsMatch(authRegisterDto.phoneNumber, phoneRegex))
                        {
                            results.Add($"Thất bại: Số điện thoại không hợp lệ cho user {authRegisterDto.userName}.");
                            failedCount++;
                            await transaction.RollbackAsync();
                            continue;
                        }
                    }

                    // Kiểm tra user tồn tại
                    var existingUser = await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.userName == authRegisterDto.userName);

                    if (existingUser != null)
                    {
                        results.Add($"Thất bại: Người dùng {authRegisterDto.userName} đã tồn tại.");
                        failedCount++;
                        await transaction.RollbackAsync();
                        continue;
                    }

                    // Băm mật khẩu
                    var (passwordHash, passwordSalt) = PasswordHasher.HashPassword(authRegisterDto.password);
                    var verificationToken = GenerateVerificationToken();

                    // Tạo user mới
                    var user = new AuthUser
                    {
                        userName = authRegisterDto.userName,
                        passwordHash = passwordHash,
                        passwordSalt = passwordSalt,
                        email = authRegisterDto.email,
                        phoneNumber = authRegisterDto.phoneNumber,
                        fullName = authRegisterDto.fullName,
                        avatar = "",
                        dateOfBirth = authRegisterDto.dateOfBirth,
                        gender = authRegisterDto.gender,
                        roleID = authRegisterDto.roleID,
                        token = "",
                        isEmailVerified = false,
                        isEmp = true,
                        emailVerificationToken = verificationToken
                    };

                    await _dbContext.Users.AddAsync(user);
                    await _dbContext.SaveChangesAsync();

                    // Lấy user vừa tạo
                    var userRes = await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.userName == authRegisterDto.userName);

                    if (userRes == null)
                    {
                        results.Add($"Thất bại: Không thể tìm thấy người dùng vừa tạo cho user {authRegisterDto.userName}.");
                        failedCount++;
                        await transaction.RollbackAsync();
                        continue;
                    }

                    // Tạo employee
                    var emp = new AuthCreateEmpDto
                    {
                        userID = userRes.userID,
                        positionID = authRegisterDto.positionID,
                        specialtyID = authRegisterDto.specialtyID,
                        startDate = authRegisterDto.startDate,
                        branchID = authRegisterDto.branchID,
                    };

                    var empResult = await _authEmpService.AuthCreateEmp(emp);
                    if (empResult.ErrorCode != 200)
                    {
                        results.Add($"Thất bại: Tạo employee thất bại cho user {authRegisterDto.userName}.");
                        failedCount++;
                        await transaction.RollbackAsync();
                        continue;
                    }

                    await _dbContext.SaveChangesAsync();

                    // Gửi email xác thực
                    await _emailService.SendVerificationEmail(userRes.userID, userRes.email, verificationToken);

                    await transaction.CommitAsync();
                    results.Add($"Thành công: Đăng ký thành công cho user {authRegisterDto.userName}. Vui lòng kiểm tra email để xác nhận.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error in AuthRegisterEmp for user {UserName}: {Message}", authRegisterDto.userName, ex.Message);
                    results.Add($"Thất bại: Lỗi hệ thống khi đăng ký user {authRegisterDto.userName}.");
                    failedCount++;
                }
            }

            var message = failedCount == 0
                ? "Đăng ký tất cả tài khoản thành công."
                : $"Đăng ký hoàn tất với {authRegisterDtos.Count - failedCount} tài khoản thành công, {failedCount} tài khoản thất bại.";

            return ErrorConst.Success(message, results);
        }

        public async Task<ResponeDto> AuthLogin(AuthLoginDto authLoginDto)
        {
            _logger.LogInformation("AuthLogin");
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.userName == authLoginDto.userName);
                if (user == null)
                {
                    return ErrorConst.Error(404, "Người dùng không tồn tại.");
                }
                if (user.isEmp == true)
                {
                    return ErrorConst.Error(500, "ban khong co quyen truy cap ");
                }

                bool isPasswordValid = PasswordHasher.VerifyPassword(authLoginDto.password, user.passwordHash, user.passwordSalt);
                if (!isPasswordValid)
                {
                    return ErrorConst.Error(401, "Mật khẩu không đúng.");
                }

                if (!user.isEmailVerified)
                {
                    return ErrorConst.Error(403, "Vui lòng xác minh email trước khi đăng nhập.");
                }

                string cacheKey = $"user_{authLoginDto.userName}";
                var cachedToken = await _redisDb.StringGetAsync(cacheKey);
                if (cachedToken.HasValue)
                {
                    return ErrorConst.Success("Đăng nhập thành công", cachedToken.ToString());
                }

                var authPermissions = await _dbContext.RolePermissions
                    .Where(x => x.roleID == user.roleID)
                    .Include(x => x.AuthPermission)
                    .Select(x => x.AuthPermission)
                    .ToListAsync();

                string token = GenerateToken(user.fullName, user.userID, authPermissions);
                user.token = token;

                await _redisDb.StringSetAsync(cacheKey, token, TimeSpan.FromHours(1));
                await _dbContext.SaveChangesAsync();

                return ErrorConst.Success("Đăng nhập thành công", token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> AuthLoginEmp(AuthLoginDto authLoginDto)
        {
            _logger.LogInformation("AuthLogin");
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.userName == authLoginDto.userName);
                if (user == null)
                {
                    return ErrorConst.Error(404, "Người dùng không tồn tại.");
                }

                bool isPasswordValid = PasswordHasher.VerifyPassword(authLoginDto.password, user.passwordHash, user.passwordSalt);
                if (!isPasswordValid)
                {
                    return ErrorConst.Error(401, "Mật khẩu không đúng.");
                }

                if (!user.isEmailVerified)
                {
                    return ErrorConst.Error(403, "Vui lòng xác minh email trước khi đăng nhập.");
                }

                string cacheKey = $"user_{authLoginDto.userName}";
                var cachedToken = await _redisDb.StringGetAsync(cacheKey);
                if (cachedToken.HasValue)
                {
                    return ErrorConst.Success("Đăng nhập thành công", cachedToken.ToString());
                }

                var authPermissions = await _dbContext.RolePermissions
                    .Where(x => x.roleID == user.roleID)
                    .Include(x => x.AuthPermission)
                    .Select(x => x.AuthPermission)
                    .ToListAsync();

                string token = GenerateToken(user.fullName, user.userID, authPermissions);
                user.token = token;

                await _redisDb.StringSetAsync(cacheKey, token, TimeSpan.FromHours(1));
                await _dbContext.SaveChangesAsync();

                return ErrorConst.Success("Đăng nhập thành công", token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task Logout(string token)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.token == token);

            if (user != null)
            {
                string cacheKey = $"user_{user.userName}";
                await _redisDb.KeyDeleteAsync(cacheKey);

                user.token = null;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ResponeDto> VerifyEmail(int userId, string token)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
                    x.userID == userId &&
                    x.emailVerificationToken == token);

                if (user == null)
                {
                    return ErrorConst.Error(400, "Invalid verification token or user not found.");
                }

                if (user.isEmailVerified)
                {
                    return ErrorConst.Error(400, "Email already verified.");
                }

                user.isEmailVerified = true;
                user.emailVerificationToken = "";
                await _dbContext.SaveChangesAsync();

                return ErrorConst.Success("Email verified successfully.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> AuthUpdateUser(AuthUpdateUserDto authUpdateUserDto)
        {
            _logger.LogInformation("AuthUpdateUser");
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.userID == authUpdateUserDto.userID);
                if (user == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy người dùng");
                }

                user.email = authUpdateUserDto.email;
                user.phoneNumber = authUpdateUserDto.phoneNumber;
                user.fullName = authUpdateUserDto.fullName;
                user.avatar = authUpdateUserDto.avatar;
                user.dateOfBirth = authUpdateUserDto.dateOfBirth;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật thông tin thành công", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> AuthGetUser(int userID)
        {
            _logger.LogInformation("AuthGetUser");
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.userID == userID);
                if (user == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy người dùng");
                }
                return ErrorConst.Success("Lấy thông tin người dùng thành công", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> AuthDeleteUser(int userID)
        {
            _logger.LogInformation("AuthDeleteUser");
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.userID == userID);
                if (user == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy người dùng");
                }
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa người dùng thành công", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> AuthGetAllUser()
        {
            _logger.LogInformation("AuthGetAllUser");
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                return ErrorConst.Success("Lấy danh sách người dùng thành công", users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> GetUserToOtherDomain(int userID)
        {
            _logger.LogInformation("GetUserToOtherDomain");
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.userID == userID);
                if (user == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy người dùng");
                }
                return ErrorConst.Success("Lấy thông tin người dùng thành công", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> AuthUpdateUserAvatar ( int userID , IFormFile file)
        {
            _logger.LogInformation("Update user avatar");
            try
            {
                var user = await _dbContext.Users.FindAsync(userID);
                if(user == null )
                {
                    return ErrorConst.Error(500, "khong tim thay user");
                }
                var result = await _cloudinaryService.UploadImageAsync(file);
                if(result == null)
                {
                    return ErrorConst.Error(500, "khong the upload file");
                }
                user.avatar = result;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat avatar thanh cong", null);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthChangeUserFullName(int userID , string FullName)
        {
            _logger.LogInformation("Update user FullName");
            try
            {
                var user = await _dbContext.Users.FindAsync(userID);
                if (user == null)
                {
                    return ErrorConst.Error(500, "khong tim thay user");
                }
                user.fullName = FullName;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat avatar thanh cong", null);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
       }
}