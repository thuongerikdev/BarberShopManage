using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class AuthCreateCustomerDto
    {
        public int userID { get; set; }
        public string? customerType { get; set; }
        public string? customerStatus { get; set; }
        public int vipID { get; set; }

    }
    public class AuthReadCustomerDto
    {
        public int customerID { get; set; }
        public int userID { get; set; }
        public int loyaltyPoints { get; set; }
        public string? customerType { get; set; }
        public string? customerStatus { get; set; }
    }
    public class AuthUpdateCustomerDto
    {
        public int customerID { get; set; }
        public string? customerType { get; set; }
        public string? customerStatus { get; set; }
    }
    public class AuthUpdateVipCustomerDto
    {
        public int customerID { get; set; }
        public double totalAmount { get; set; }
    }
}
