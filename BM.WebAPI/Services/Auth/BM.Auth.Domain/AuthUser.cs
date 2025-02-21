using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
    [Table(nameof(AuthUser), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userID { get; set; }
        [MaxLength(50)]
        public string userName { get; set; }
        [MaxLength(50)]
        public string password { get; set; }
        [MaxLength(10)]
        public string email { get; set; }
        [MaxLength(20)]
        public string phoneNumber { get; set; }
        [MaxLength(50)]
        public string fullName { get; set; }
        public string avatar { get; set; }
        public DateTime? dateOfBirth { get; set; }
        [MaxLength(10)]
        public string gender { get; set; }

        public virtual AuthEmp AuthEmp { get; set; }
        public virtual AuthCustomer AuthCustomer { get; set; }
    }
}
