using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
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
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public virtual ICollection<AuthScheEmp> AuthScheEmps { get; set; }



    }
}
