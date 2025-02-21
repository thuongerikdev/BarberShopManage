using BM.Auth.Dtos;
using BM.Constant;
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
        public Task<ResponeDto> AuthUpdateUser(AuthUpdateUserDto authUpdateUserDto);
        public Task<ResponeDto> AuthDeleteUser(int userID);
        public Task<ResponeDto> AuthGetUser(int userID);
        public Task<ResponeDto> AuthGetAllUser();

    }
}
