using BM.Auth.Dtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Abtracts
{
    public interface IAuthEmpService
    {
        public Task<ResponeDto> AuthCreateEmp(AuthCreateEmpDto authCreateEmpDto);
        public Task<ResponeDto> AuthUpdateEmp(AuthUpdateEmpDto authUpdateEmpDto);
        public Task<ResponeDto> AuthDeleteEmp(int empID);
        public Task<ResponeDto> AuthGetEmp(int empID);
        public Task<ResponeDto> AuthGetAllEmp();
    }
}
