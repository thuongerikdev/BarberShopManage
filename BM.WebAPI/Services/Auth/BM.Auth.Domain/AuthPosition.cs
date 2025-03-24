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
    [Table(nameof(AuthSchedule), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int scheduleID { get; set; }
        public string? scheduleName { get; set; }
        public string? description { get; set; }
        public string? note { get; set; }
        public int percent { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public virtual ICollection<AuthScheEmp> AuthScheEmps { get; set; }

    }
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
    [Table(nameof(AuthScheEmp), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthScheEmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int scheEmpID { get; set; }
        public int empID { get; set; }
        public int scheduleID { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string? status { get; set; }
        public string? note { get; set; }
        public virtual AuthEmp AuthEmp { get; set; }
        public virtual AuthSchedule AuthSchedule { get; set; }


    }
}
