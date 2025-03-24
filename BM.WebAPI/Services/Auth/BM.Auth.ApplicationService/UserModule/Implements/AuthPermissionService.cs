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
    public class AuthPermissionService : AuthServiceBase , IAuthPermissionService
    {
        public AuthPermissionService(ILogger<AuthPermissionService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreatePermission(AuthCreatePermissionDto authCreatePermissionDto)
        {
            _logger.LogInformation("AuthCreatePermission");
            try
            {
                var permission = new AuthPermission
                {
                    permissionName = authCreatePermissionDto.permissionName,
                    permissionDes = authCreatePermissionDto.permissionDes,
                    status = authCreatePermissionDto.status,
                    startDate = DateTime.Now,
                    updateAt = DateTime.Now

                };
                _dbContext.Permissions.Add(permission);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo permission thành công", permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdatePermission(AuthUpdatePermissionDto authUpdatePermissionDto)
        {
            _logger.LogInformation("AuthUpdatePermission");
            try
            {
                var permission = _dbContext.Permissions.FirstOrDefault(x => x.permissionID == authUpdatePermissionDto.permissionID);
                if (permission == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy permission");
                }
                permission.permissionName = authUpdatePermissionDto.permissionName;
                permission.permissionDes = authUpdatePermissionDto.permissionDes;
                permission.status = authUpdatePermissionDto.status;
                permission.updateAt = DateTime.Now;
                permission.startDate = authUpdatePermissionDto.startDate;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật permission thành công", permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeletePermission(int permissionID)
        {
            _logger.LogInformation("AuthDeletePermission");
            try
            {
                var permission = _dbContext.Permissions.FirstOrDefault(x => x.permissionID == permissionID);
                if (permission == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy permission");
                }
                _dbContext.Permissions.Remove(permission);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa permission thành công", permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetPermission(int permissionID)
        {
            _logger.LogInformation("AuthGetPermission");
            try
            {
                var permission = _dbContext.Permissions.FirstOrDefault(x => x.permissionID == permissionID);
                if (permission == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy permission");
                }
                return ErrorConst.Success("Lấy permission thành công", permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllPermission()
        {
            _logger.LogInformation("AuthGetAllPermission");
            try
            {
                var permissions = _dbContext.Permissions.ToList();
                return ErrorConst.Success("Lấy danh sách permission thành công", permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
