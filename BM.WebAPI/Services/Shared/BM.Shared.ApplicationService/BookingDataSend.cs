using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Shared.ApplicationService
{
    public interface ISendOrderData
    {
        public Task<ResponeDto> GetOrderToOtherDomain(int orderID);
    }
}
