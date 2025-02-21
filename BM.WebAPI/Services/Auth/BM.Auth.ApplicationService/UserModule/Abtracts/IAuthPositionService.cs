using BM.Auth.Dtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Abtracts
{
    public interface IAuthPositionService
    {
        public Task<ResponeDto> AuthCreatePosition(AuthCreatePositionDto authCreatePositionDto);
        public Task<ResponeDto> AuthUpdatePosition(AuthUpdatePositionDto authUpdatePositionDto);
        public Task<ResponeDto> AuthDeletePosition(int positionID);
        public Task<ResponeDto> AuthGetPosition(int positionID);
        public Task<ResponeDto> AuthGetAllPosition();
    }
}
