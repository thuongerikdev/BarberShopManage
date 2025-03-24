using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Constant
{
    public static class PermissionConstants
    {
        public static readonly Dictionary<string, string> Permissions = new()
         {
            { "User.Create", "/user/create" },
            { "User.Read", "/user/read" },
            { "User.Update", "/user/update" },
            { "User.Delete", "/user/delete" },
            { "Booking.Create", "/booking/create" },
            { "Booking.Read", "/booking/read" },
            { "Booking.Update", "/booking/update" },
            { "Booking.Delete", "/booking/delete" },
            { "Customer.Create", "/customer/create" },
            { "Customer.Read", "/customer/read" },
            { "Customer.Update", "/customer/update" },
            { "Customer.Delete", "/customer/delete" },
            { "Role.Create", "/role/create" },
            { "Role.Read", "/role/read" },
            { "Role.Update", "/role/update" },
            { "Role.Delete", "/role/delete" },
            { "Permission.Create", "/permission/create" },
            { "Permission.Read", "/permission/read" },
            { "Permission.Update", "/permission/update" },
            { "Permission.Delete", "/permission/delete" },
            { "Specialty.Create", "/specialty/create" },
            { "Specialty.Read", "/specialty/read" },
            { "Specialty.Update", "/specialty/update" },
            { "Specialty.Delete", "/specialty/delete" },
            { "Position.Create", "/position/create" },
            { "Position.Read", "/position/read" },
            { "Position.Update", "/position/update" },
            { "Position.Delete", "/position/delete" },
            { "Auth.Create", "/auth/create" },
            { "Auth.Read", "/auth/read" },
            { "Auth.Update", "/auth/update" },
            { "Auth.Delete", "/auth/delete" },
            { "Auth.CreateRolePermission", "/auth/createrolepermission" },
            { "Auth.ReadRolePermission", "/auth/readrolepermission" },
            { "Auth.UpdateRolePermission", "/auth/updaterolepermission" },
            { "Auth.CreateEmp", "/auth/createemp" },
            { "Auth.ReadEmp", "/auth/reademp" },
            { "Auth.UpdateEmp", "/auth/updateemp" },
            { "Auth.CreateCustomer", "/auth/createcustomer" },
            { "Auth.ReadCustomer", "/auth/readcustomer" },
            { "Auth.UpdateCustomer", "/auth/updatecustomer" },
            { "Auth.CreateRole", "/auth/createrole" },
            { "Auth.ReadRole", "/auth/readrole" },
            { "Auth.UpdateRole", "/auth/updaterole" },
            { "Auth.CreatePermission", "/auth/createpermission" },
            { "Auth.ReadPermission", "/auth/readpermission" },
            { "Auth.UpdatePermission", "/auth/updatepermission" },
            { "Auth.CreateSpecialty", "/auth/createspecialty" },
       
         };
    }
}
