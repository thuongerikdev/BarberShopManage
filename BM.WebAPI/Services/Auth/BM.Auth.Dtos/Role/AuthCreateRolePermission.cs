using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos.Role
{
    public class AuthCreateRolePermissionDto
    {
        public int roleID { get; set; }
        public int permissionID { get; set; }
        public string? status { get; set; }
    }
    public class AuthReadRolePermissionDto
    {
        public int rolePermissionID { get; set; }
        public int roleID { get; set; }
        public int permissionID { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
    }
    public class AuthUpdateRolePermissionDto
    {
        public int rolePermissionID { get; set; }
        public int roleID { get; set; }
        public int permissionID { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
    }
}
