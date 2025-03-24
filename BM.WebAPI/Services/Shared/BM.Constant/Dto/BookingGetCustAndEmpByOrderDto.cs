using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BM.Constant.Dto
{
    public class BookingGetCustAndEmpByOrderDto
    {
        public int custID { get; set; }
        public double totalMoney { get; set; }
        public List <BookingEmpMoney> empMoney { get; set; }
    }
    public class BookingEmpMoney
    { 
        public int empID { get; set; }
        public double money { get; set; }
        //public int rating { get; set; }
      
    }
}
