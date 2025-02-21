using BM.Auth.Dtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Abtracts
{
    public interface IAuthScheEmpService
    {
        public Task<ResponeDto> AuthCreateScheEmp(AuthCreateScheEmpDto authCreateScheEmpDto);
        public Task<ResponeDto> AuthUpdateScheEmp(AuthUpdateScheEmpDto authUpdateScheEmpDto);
        public Task<ResponeDto> AuthDeleteScheEmp(int scheEmpID);
        public Task<ResponeDto> AuthGetScheEmp(int scheEmpID);
        public Task<ResponeDto> AuthGetAllScheEmp();
        public Task<ResponeDto> AuthGetScheEmpByEmpID(int empID);
        public Task<ResponeDto> AuthGetScheEmpByScheID(int scheID);
        public Task<ResponeDto> AuthGetScheEmpByMonth(int month ,int empID);
        public Task<ResponeDto> AuthGetAllScheduleWeekNow(int weekOffset = 0);
    }
}
