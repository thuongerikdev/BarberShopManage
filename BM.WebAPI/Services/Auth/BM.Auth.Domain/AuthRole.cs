using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
    [Table(nameof(AuthRole), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int roleID { get; set; }
        [MaxLength(50)]
        public string roleName { get; set; }
        [MaxLength(50)]
        public string roleDes { get; set; }
        [MaxLength(50)]
        public string status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
        public virtual ICollection<AuthUser> AuthUsers { get; set; }
        public virtual ICollection<AuthRolePermission> AuthRolePermissions { get; set; }


    }
    [Table(nameof(AuthRolePermission), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthRolePermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rolePermissionID { get; set; }
        public int roleID { get; set; }
        public int permissionID { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
        public virtual AuthRole AuthRole { get; set; }
        public virtual AuthPermission AuthPermission { get; set; }

    }
    [Table(nameof(AuthPermission), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int permissionID { get; set; }
        [MaxLength(50)]
        public string permissionName { get; set; }
        [MaxLength(50)]
        public string permissionDes { get; set; }
        [MaxLength(50)]
        public string status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? updateAt { get; set; }
        public virtual ICollection<AuthRolePermission> AuthRolePermissions { get; set; }

    }

}
