using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos.Role
{
    public class AuthCreatePermissionDto
    {
        public string permissionName { get; set; }
        public string permissionDes { get; set; }
        public string status { get; set; }
    }
    public class AuthReadPermissionDto
    {
        public int permissionID { get; set; }
        public string permissionName { get; set; }
        public string permissionDes { get; set; }
        public string status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
    }
    public class AuthUpdatePermissionDto
    {
        public int permissionID { get; set; }
        public string permissionName { get; set; }
        public string permissionDes { get; set; }
        public string status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
    }

}
