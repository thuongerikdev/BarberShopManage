using BM.Auth.Dtos;
using BM.Auth.Dtos.User;
using BM.Constant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Abtracts
{
    public interface IAuthUserService
    {
        public Task<ResponeDto> AuthLogin(AuthLoginDto authLoginDto);
        public Task<ResponeDto> AuthRegister(AuthRegisterDto authRegisterDto);
        public Task<ResponeDto> AuthRegisterEmp(AuthRegisteEmprDto authRegisterDto);
        public Task<ResponeDto> AuthUpdateUser(AuthUpdateUserDto authUpdateUserDto);
        public Task<ResponeDto> AuthDeleteUser(int userID);
        public Task<ResponeDto> VerifyEmail(int userId, string token);
        public Task<ResponeDto> AuthGetUser(int userID);
        public Task<ResponeDto> AuthGetAllUser();
        public Task<ResponeDto> AuthUpdateUserAvatar(int userID, IFormFile file);

    }
    public interface IAuthCustomerService
    {
        public Task<ResponeDto> AuthCreateCustomer(AuthCreateCustomerDto authCreateCustomerDto);
        public Task<ResponeDto> AuthUpdateCustomer(AuthUpdateCustomerDto authUpdateCustomerDto);
        public Task<ResponeDto> AuthDeleteCustomer(int customerID);
        public Task<ResponeDto> AuthGetCustomer(int customerID);
        public Task<ResponeDto> AuthGetAllCustomer();
    }
    public interface IAuthEmpService
    {
        public Task<ResponeDto> AuthCreateEmp(AuthCreateEmpDto authCreateEmpDto);
        public Task<ResponeDto> AuthUpdateEmp(AuthUpdateEmpDto authUpdateEmpDto);
        public Task<ResponeDto> AuthDeleteEmp(int empID);
        public Task<ResponeDto> AuthGetEmp(int empID);
        public Task<ResponeDto> AuthGetAllEmp();
        public Task<ResponeDto> AuthGetAllUserEmp();
        //public Task<ResponeDto> CaculateSalary (int positionID, int specialtyID);
    }
    
}
