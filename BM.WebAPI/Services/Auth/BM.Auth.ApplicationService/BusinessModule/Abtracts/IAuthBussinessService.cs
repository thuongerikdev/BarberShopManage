using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.BusinessModule.Abtracts
{
    public interface IAuthBussinessService
    {
        public Task<ResponeDto> AuthFinshedPayment(int orderID);
    }
}
