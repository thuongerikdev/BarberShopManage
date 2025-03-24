using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos.Role;
using BM.Auth.Infrastructure;
using BM.Constant;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Implements
{
    public class AuthRoleService : AuthServiceBase, IAuthRoleService
    {
        public AuthRoleService(ILogger<AuthRoleService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreateRole(AuthCreateRoleDto authCreateRoleDto)
        {
            _logger.LogInformation("AuthCreateRole");
            try
            {
                var role = new AuthRole
                {
                    roleName = authCreateRoleDto.roleName,
                    roleDes = authCreateRoleDto.roleDes,
                    status = authCreateRoleDto.status,

                };
                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo role thành công", role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateRole(AuthUpdateRoleDto authUpdateRoleDto)
        {
            _logger.LogInformation("AuthUpdateRole");
            try
            {
                var role = _dbContext.Roles.FirstOrDefault(x => x.roleID == authUpdateRoleDto.roleID);
                if (role == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy role");
                }
                role.roleName = authUpdateRoleDto.roleName;
                role.roleDes = authUpdateRoleDto.roleDes;
                role.status = authUpdateRoleDto.status;
                role.updateAt = DateTime.Now;
                role.startDate = authUpdateRoleDto.startDate;

                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật role thành công", role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteRole(int roleID)
        {
            _logger.LogInformation("AuthDeleteRole");
            try
            {
                var role = _dbContext.Roles.FirstOrDefault(x => x.roleID == roleID);
                if (role == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy role");
                }
                _dbContext.Roles.Remove(role);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa role thành công", role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetRole(int roleID)
        {
            _logger.LogInformation("AuthGetRole");
            try
            {
                var role = _dbContext.Roles.FirstOrDefault(x => x.roleID == roleID);
                if (role == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy role");
                }
                return ErrorConst.Success("Lấy role thành công", role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllRole()
        {
            _logger.LogInformation("AuthGetAllRole");
            try
            {
                var roles = _dbContext.Roles.ToList();
                return ErrorConst.Success("Lấy danh sách role thành công", roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
