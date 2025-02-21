using BM.Auth.Dtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Abtracts
{
    public interface IAuthScheduleService
    {
        public Task<ResponeDto> AuthCreateSchedule(AuthCreateScheduleDto authCreateScheduleDto);
        public Task<ResponeDto> AuthUpdateSchedule(AuthUpdateScheduleDto authUpdateScheduleDto);
        public Task<ResponeDto> AuthDeleteSchedule(int scheduleID);
        public Task<ResponeDto> AuthGetSchedule(int scheduleID);
        public Task<ResponeDto> AuthGetAllSchedule();
       
    }
}
