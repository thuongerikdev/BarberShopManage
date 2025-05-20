using BM.Auth.Dtos;
using BM.Auth.Dtos.Position;
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
    public interface IAuthScheduleService
    {
        public Task<ResponeDto> AuthCreateSchedule(List<AuthCreateScheduleDto> authCreateScheduleDto);
        public Task<ResponeDto> AuthUpdateSchedule(AuthUpdateScheduleDto authUpdateScheduleDto);
        public Task<ResponeDto> AuthDeleteSchedule(int scheduleID);
        public Task<ResponeDto> AuthGetSchedule(int scheduleID);
        public Task<ResponeDto> AuthGetAllSchedule();
        Task<ResponeDto> GetEmployeesByDateAndBranch(DateTime date, int branchesID, string typeOfEmp);

    }
    public interface IAuthSpecService
    {
        public Task<ResponeDto> AuthCreateSpec(AuthCreateSpecDto authCreateSpecDto);
        public Task<ResponeDto> AuthUpdateSpec(AuthUpdateSpecDto authUpdateSpecDto);
        public Task<ResponeDto> AuthDeleteSpec(int specialtyID);
        public Task<ResponeDto> AuthGetSpec(int specialtyID);
        public Task<ResponeDto> AuthGetAllSpec();
    }
    public interface IAuthScheEmpService
    {
        public Task<ResponeDto> AuthCreateScheEmp(List<AuthCreateScheEmpDto> authCreateScheEmpDto);
        public Task<ResponeDto> AuthUpdateScheEmp(AuthUpdateScheEmpDto authUpdateScheEmpDto);
        public Task<ResponeDto> AuthDeleteScheEmp(int scheEmpID);
        public Task<ResponeDto> AuthGetScheEmp(int scheEmpID);
        public Task<ResponeDto> AuthGetAllScheEmp();
        public Task<ResponeDto> AuthGetScheEmpByEmpID(int empID);
        public Task<ResponeDto> AuthGetScheEmpByScheID(int scheID);
        public Task<ResponeDto> AuthGetScheEmpByMonth(int month, int empID);
        public Task<ResponeDto> AuthGetAllScheduleWeekNow(int weekOffset = 0);
        Task<ResponeDto> AuthGetEmployeeSchedule(int empID);
    }
}
