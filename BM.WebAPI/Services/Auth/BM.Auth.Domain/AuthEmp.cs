using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
    [Table(nameof(AuthEmp), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthEmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int empID { get; set; }
        public int userID { get; set; }
        public int positionID { get; set; }
        public int specialtyID { get; set; }
        public string empCode { get; set; }
        public double salary { get; set; }
        public double rate { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }


        public virtual AuthUser AuthUser { get; set; }
        public virtual AuthPosition AuthPosition { get; set; }
        public virtual AuthSpecialty AuthSpecialty { get; set; }
        public virtual ICollection<AuthScheEmp> AuthScheEmps { get; set; }






    }
}
