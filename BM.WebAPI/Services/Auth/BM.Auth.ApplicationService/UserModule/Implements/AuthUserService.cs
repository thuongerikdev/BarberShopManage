using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Implements
{
    public class AuthUserService :  AuthServiceBase ,IAuthUserService
    {
        private readonly IConfiguration _configuration;
        public AuthUserService( ILogger<AuthUserService> logger  , AuthDbContext dbContext , IConfiguration configuration) : base(logger, dbContext)
        {
            _configuration = configuration;
        }
        private string SecretKey => _configuration["Jwt:SecretKey"];

        private string GenerateToken(string name, int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim("name", name),
                new Claim("userId", userId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResponeDto> AuthLogin(AuthLoginDto authLoginDto)
        {
            _logger.LogInformation("AuthLogin");
            try
            {   
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.userName == authLoginDto.userName && x.password == authLoginDto.password);
                if (user == null)
                {
                    return ErrorConst.Error(500, "Sai thông tin đăng nhập hoặc mật khẩu");
                }
                string token = GenerateToken(user.fullName, user.userID);
                return ErrorConst.Success("Đăng nhập thành công", token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        
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
                var user = new AuthUser
                {
                    userName = authRegisterDto.userName,
                    password = authRegisterDto.password,
                    email = authRegisterDto.email,
                    phoneNumber = authRegisterDto.phoneNumber,
                    fullName = authRegisterDto.fullName,
                    avatar = "",
                    dateOfBirth = authRegisterDto.dateOfBirth,
                    gender = authRegisterDto.gender,
                    

                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Đăng ký thành công", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task <ResponeDto> AuthUpdateUser (AuthUpdateUserDto authUpdateUserDto)
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
        public async Task <ResponeDto> AuthGetUser (int userID)
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

    }
}
