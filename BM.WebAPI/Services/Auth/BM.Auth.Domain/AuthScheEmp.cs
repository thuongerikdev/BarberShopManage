using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
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
