using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
    [Table(nameof(AuthPosition), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthPosition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int positionID { get; set; }
        [MaxLength(50)]
        public string positionName { get; set; }
        [MaxLength(50)]
        public string description { get; set; }
        [MaxLength(50)]
        public string note { get; set; }
        [MaxLength(50)]
        public string status { get; set; }
        public double DefaultSalary { get; set; }
        public virtual ICollection<AuthEmp> AuthEmps { get; set; }

    }
}
