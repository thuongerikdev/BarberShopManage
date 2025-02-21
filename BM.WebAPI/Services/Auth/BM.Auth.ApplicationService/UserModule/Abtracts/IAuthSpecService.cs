using BM.Auth.Dtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Abtracts
{
    public interface IAuthSpecService
    {
        public Task<ResponeDto> AuthCreateSpec(AuthCreateSpecDto authCreateSpecDto);
        public Task<ResponeDto> AuthUpdateSpec(AuthUpdateSpecDto authUpdateSpecDto);
        public Task<ResponeDto> AuthDeleteSpec(int specialtyID);
        public Task<ResponeDto> AuthGetSpec(int specialtyID);
        public Task<ResponeDto> AuthGetAllSpec();
    }
}
