using BM.Auth.Dtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Abtracts
{
    public interface IAuthCustomerService
    {
        public Task<ResponeDto> AuthCreateCustomer(AuthCreateCustomerDto authCreateCustomerDto);
        public Task<ResponeDto> AuthUpdateCustomer(AuthUpdateCustomerDto authUpdateCustomerDto);
        public Task<ResponeDto> AuthDeleteCustomer(int customerID);
        public Task<ResponeDto> AuthGetCustomer(int customerID);
        public Task<ResponeDto> AuthGetAllCustomer();
    }
}
