using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
    [Table(nameof(AuthCustomer), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int customerID { get; set; }
        public int userID { get; set; }
        public int loyaltyPoints { get; set; }
        public string? customerType { get; set; }
        public string? customerStatus { get; set; }
        public virtual AuthUser AuthUser { get; set; }


    }
}
