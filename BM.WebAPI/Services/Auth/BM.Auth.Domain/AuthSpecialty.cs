using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
    [Table(nameof(AuthSpecialty), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthSpecialty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int specialtyID { get; set; }
        [MaxLength(50)]
        public string specialtyName { get; set; }
        [MaxLength(50)]
        public string description { get; set; }
        [MaxLength(50)]
        public string note { get; set; }
        [MaxLength(50)]
        public string status { get; set; }
        public int percent { get; set; }
        public virtual ICollection<AuthEmp> AuthEmps { get; set; }

    }
}
