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
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            _logger.LogInformation("AuthRegister");
            try
            {
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.userName == authRegisterDto.userName);
                if (existingUser != null)
                {
                    return ErrorConst.Error(500, "Người dùng đã tồn tại.");
                }

                // Băm mật khẩu
                var (passwordHash, passwordSalt) = PasswordHasher.HashPassword(authRegisterDto.password);

                var verificationToken = GenerateVerificationToken();

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
                    isEmp = false ,
                    emailVerificationToken = verificationToken
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                

             

                var userRes = await _dbContext.Users.FirstOrDefaultAsync(x => x.userName == authRegisterDto.userName);
                if (userRes == null)
                {
                    return ErrorConst.Error(500, "Không thể tìm thấy người dùng vừa tạo.");
                }

                var customer = new AuthCreateCustomerDto
                {
                    userID = userRes.userID,
                    customerStatus = "OK",
                    customerType = "Normal",
                    vipID = 1


                };
                await _authCustomerService.AuthCreateCustomer(customer);
                await _dbContext.SaveChangesAsync();

                await _emailService.SendVerificationEmail(userRes.userID, userRes.email, verificationToken);
                return ErrorConst.Success("Đăng ký thành công. Vui lòng kiểm tra email để xác nhận.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthRegisterEmp(AuthRegisteEmprDto authRegisterDto)
        {
            _logger.LogInformation("AuthRegister");
            try
            {
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.userName == authRegisterDto.userName);
                if (existingUser != null)
                {
                    return ErrorConst.Error(500, "Người dùng đã tồn tại.");
                }

                // Băm mật khẩu
                var (passwordHash, passwordSalt) = PasswordHasher.HashPassword(authRegisterDto.password);

                var verificationToken = GenerateVerificationToken();

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
               

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                var userRes = await _dbContext.Users.FirstOrDefaultAsync(x => x.userName == authRegisterDto.userName);

                var emp = new AuthCreateEmpDto
                {
                    userID = userRes.userID,
                    positionID = authRegisterDto.positionID,
                    specialtyID = authRegisterDto.specialtyID,
                    startDate = authRegisterDto.startDate,
                    branchID = authRegisterDto.branchID,
                };
                await _authEmpService.AuthCreateEmp(emp);
                await _dbContext.SaveChangesAsync();
                if (userRes == null)
                {
                    return ErrorConst.Error(500, "Không thể tìm thấy người dùng vừa tạo.");
                }

                await _emailService.SendVerificationEmail(userRes.userID, userRes.email, verificationToken);
                return ErrorConst.Success("Đăng ký thành công. Vui lòng kiểm tra email để xác nhận.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
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
    }
}