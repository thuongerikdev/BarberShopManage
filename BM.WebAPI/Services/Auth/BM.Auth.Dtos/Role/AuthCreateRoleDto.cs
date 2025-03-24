using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos.Role
{
    public class AuthCreateRoleDto
    {
        public string roleName { get; set; }
        public string roleDes { get; set; }
        public string status { get; set; }

    }
    public class AuthReadRoleDto
    {
        public int roleID { get; set; }
        public string roleName { get; set; }
        public string roleDes { get; set; }
        public string status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
    }
    public class AuthUpdateRoleDto
    {
        public int roleID { get; set; }
        public string roleName { get; set; }
        public string roleDes { get; set; }
        public string status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
    }
}
