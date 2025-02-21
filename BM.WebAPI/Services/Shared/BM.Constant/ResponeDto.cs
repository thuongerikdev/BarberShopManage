using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Constant
{
    public  class ResponeDto
    {
        public int ErrorCode { get; set; }
        public string ErrorMessager { get; set; }
        public object Data { get; set; }
    }
}
