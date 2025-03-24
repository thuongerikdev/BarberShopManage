using BM.Auth.Dtos.Role;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Abtracts
{
    public interface IAuthRoleService
    {
        public Task<ResponeDto> AuthCreateRole(AuthCreateRoleDto authCreateRoleDto);
        public Task<ResponeDto> AuthUpdateRole(AuthUpdateRoleDto authUpdateRoleDto);
        public Task<ResponeDto> AuthDeleteRole(int roleID);
        public Task<ResponeDto> AuthGetRole(int roleID);
        public Task<ResponeDto> AuthGetAllRole();
    }
    public interface IAuthRolePermissionService
    {
        public Task<ResponeDto> AuthCreateRolePermission(AuthCreateRolePermissionDto authCreateRolePermission);
        public Task<ResponeDto> AuthUpdateRolePermission(AuthUpdateRolePermissionDto authUpdateRolePermission);
        public Task<ResponeDto> AuthDeleteRolePermission(int rolePermissionID);
        public Task<ResponeDto> AuthGetRolePermission(int rolePermissionID);
        public Task<ResponeDto> AuthGetAllRolePermission();
    }
    public interface IAuthPermissionService
    {
        public Task<ResponeDto> AuthCreatePermission(AuthCreatePermissionDto authCreatePermissionDto);
        public Task<ResponeDto> AuthUpdatePermission(AuthUpdatePermissionDto authUpdatePermissionDto);
        public Task<ResponeDto> AuthDeletePermission(int permissionID);
        public Task<ResponeDto> AuthGetPermission(int permissionID);
        public Task<ResponeDto> AuthGetAllPermission();
    }
}
