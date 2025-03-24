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
    public class AuthRolePermissionService : AuthServiceBase, IAuthRolePermissionService
    {
        public AuthRolePermissionService(ILogger<AuthRolePermissionService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreateRolePermission(AuthCreateRolePermissionDto authCreateRolePermissionDto)
        {
            _logger.LogInformation("AuthCreateRolePermission");
            try
            {
                var rolePermission = new AuthRolePermission
                {
                    roleID = authCreateRolePermissionDto.roleID,
                    permissionID = authCreateRolePermissionDto.permissionID,
                    status = authCreateRolePermissionDto.status,
                    startDate = DateTime.Now,
                    updateAt = DateTime.Now

                };
                _dbContext.RolePermissions.Add(rolePermission);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo role permission thành công", rolePermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateRolePermission(AuthUpdateRolePermissionDto authUpdateRolePermissionDto)
        {
            _logger.LogInformation("AuthUpdateRolePermission");
            try
            {
                var rolePermission = _dbContext.RolePermissions.FirstOrDefault(x => x.rolePermissionID == authUpdateRolePermissionDto.rolePermissionID);
                if (rolePermission == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy role permission");
                }
                rolePermission.roleID = authUpdateRolePermissionDto.roleID;
                rolePermission.permissionID = authUpdateRolePermissionDto.permissionID;
                rolePermission.status = authUpdateRolePermissionDto.status;
                rolePermission.updateAt = DateTime.Now;
                rolePermission.startDate = authUpdateRolePermissionDto.startDate;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật role permission thành công", rolePermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteRolePermission(int rolePermissionID)
        {
            _logger.LogInformation("AuthDeleteRolePermission");
            try
            {
                var rolePermission = _dbContext.RolePermissions.FirstOrDefault(x => x.rolePermissionID == rolePermissionID);
                if (rolePermission == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy role permission");
                }
                _dbContext.RolePermissions.Remove(rolePermission);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa role permission thành công", rolePermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetRolePermission(int rolePermissionID)
        {
            _logger.LogInformation("AuthGetRolePermission");
            try
            {
                var rolePermission = _dbContext.RolePermissions.FirstOrDefault(x => x.rolePermissionID == rolePermissionID);
                if (rolePermission == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy role permission");
                }
                return ErrorConst.Success("Lấy role permission thành công", rolePermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> AuthGetAllRolePermission()
        {
            _logger.LogInformation("GetAllRolePermission");
            try
            {
                var roles = _dbContext.RolePermissions.ToList();
                return ErrorConst.Success("Lấy danh sách role permission thành công", roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetRolePermissionByRoleID(int roleID)
        {
            _logger.LogInformation("AuthGetRolePermissionByRoleID");
            try
            {
                var rolePermission = _dbContext.RolePermissions.Where(x => x.roleID == roleID).ToList();
                if (rolePermission == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy role permission");
                }
                return ErrorConst.Success("Lấy role permission thành công", rolePermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
