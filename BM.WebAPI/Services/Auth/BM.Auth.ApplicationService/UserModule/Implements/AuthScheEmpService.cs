using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Implements
{
    public class AuthScheEmpService : AuthServiceBase, IAuthScheEmpService
    {
        protected readonly IAuthScheduleService _authScheduleService;
        protected readonly IAuthEmpService _authEmpService;
        public AuthScheEmpService(ILogger<AuthScheEmpService> logger, AuthDbContext authDbContext , IAuthScheduleService authScheduleService , IAuthEmpService authEmpService) : base(logger, authDbContext)
        {
            _authEmpService = authEmpService;
            _authScheduleService = authScheduleService;
        }
        public async Task<ResponeDto> AuthCreateScheEmp(List<AuthCreateScheEmpDto> authCreateScheEmpDto)
        {
            _logger.LogInformation("AuthCreateScheEmp");
            try
            {
                var scheEmp = authCreateScheEmpDto.Select( dto =>  new AuthScheEmp
                {
                    scheduleID = dto.scheduleID,
                    empID = dto.empID,
                    status = dto.status,
                    note = dto.note,
                    startDate = dto.startDate,
                    endDate = dto.endDate ,

                }).ToList();
                _dbContext.ScheEmps.AddRangeAsync(scheEmp);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo lịch hẹn nhân viên thành công", scheEmp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateScheEmp(AuthUpdateScheEmpDto authUpdateScheEmpDto)
        {
            _logger.LogInformation("AuthUpdateScheEmp");
            try
            {
                var scheEmp = await _dbContext.ScheEmps.FindAsync(authUpdateScheEmpDto.scheEmpID);
                if (scheEmp == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy lịch hẹn nhân viên");
                }
                scheEmp.scheduleID = authUpdateScheEmpDto.scheduleID;
                scheEmp.empID = authUpdateScheEmpDto.empID;
                scheEmp.status = authUpdateScheEmpDto.status;
                scheEmp.note = authUpdateScheEmpDto.note;
                scheEmp.startDate = authUpdateScheEmpDto.startDate;
                scheEmp.endDate = authUpdateScheEmpDto.endDate;
                
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật lịch hẹn nhân viên thành công", scheEmp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteScheEmp(int scheEmpID)
        {
            _logger.LogInformation("AuthDeleteScheEmp");
            try
            {
                var scheEmp = await _dbContext.ScheEmps.FindAsync(scheEmpID);
                if (scheEmp == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy lịch hẹn nhân viên");
                }
                _dbContext.ScheEmps.Remove(scheEmp);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa lịch hẹn nhân viên thành công", scheEmp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetScheEmp(int scheEmpID)
        {
            _logger.LogInformation("AuthGetScheEmp");
            try
            {
                var scheEmp = await _dbContext.ScheEmps.FindAsync(scheEmpID);
                if (scheEmp == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy lịch hẹn nhân viên");
                }
                return ErrorConst.Success("Lấy lịch hẹn nhân viên thành công", scheEmp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllScheEmp()
        {
            _logger.LogInformation("AuthGetAllScheEmp");
            try
            {
                var scheEmps = await _dbContext.ScheEmps.ToListAsync();
                return ErrorConst.Success("Lấy danh sách lịch hẹn nhân viên thành công", scheEmps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async  Task<ResponeDto> AuthGetScheEmpByEmpID(int empID)
        {
            _logger.LogInformation("AuthGetSchempByEmpID");
            try
            {
                var listSchedule = await _dbContext.ScheEmps
                    .Where(x => x.empID == empID)
                    .Include(x => x.AuthSchedule)
                    .Select(x => x.AuthSchedule)
                    .ToListAsync();

                //if (scheEmps == null)
                //{
                //    return ErrorConst.Error(500, "Không tìm thấy lịch hẹn nhân viên");
                //}
                //var listSchedule = new List<AuthSchedule>();
                //foreach (var sche in scheEmps)
                //{
                //    var schedule = await _dbContext.Schedules.FindAsync(sche.scheduleID);
                //    listSchedule.Add(schedule);

                //}
                return ErrorConst.Success("Lấy danh sách lịch hẹn nhân viên thành công", listSchedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetScheEmpByScheID(int scheID)
        {
            _logger.LogInformation("AuthGetSchempByScheID");
            try
            {
                var listEmp = await _dbContext.ScheEmps
                    .Where(x => x.scheduleID == scheID)
                    .Include(x => x.AuthEmp)
                    .Select(x => x.AuthEmp)
                    .ToListAsync();

                return ErrorConst.Success("Lấy danh sách lịch hẹn nhân viên thành công", listEmp);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetScheEmpByMonth(int month, int empID)
        {
            _logger.LogInformation("AuthGetSchempByMonth");
            try
            {
                var listSchedule = await _dbContext.ScheEmps
                    .Where(sche => sche.empID == empID)
                    .Include(sche => sche.AuthSchedule)
                    .Where(joined => joined.AuthSchedule.startDate.HasValue && joined.AuthSchedule.startDate.Value.Month == month)
                    .Select(joined => joined.AuthSchedule)
                    .ToListAsync();

                if (!listSchedule.Any())
                {
                    return ErrorConst.Error(404, "Không tìm thấy lịch hẹn nhân viên");
                }

                return ErrorConst.Success("Lấy danh sách lịch hẹn nhân viên thành công", listSchedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllScheduleWeekNow(int weekOffset)
        {
            _logger.LogInformation("AuthGetAllScheduleWeekNow");
            try
            {
                var weekDays = new HashSet<DateTime>(GetWeekDays(DateTime.Now, weekOffset));

                var scheduleData = await _dbContext.Schedules
                    .Where(sche => sche.startDate.HasValue && weekDays.Contains(sche.startDate.Value.Date))
                    .Include(sche => sche.AuthScheEmps)
                    .ThenInclude(se => se.AuthEmp)
                    .ToListAsync();

                if (!scheduleData.Any())
                {
                    return ErrorConst.Error(404, "Không tìm thấy lịch hẹn trong tuần này");
                }

                var scheduleArray = new List<object[]>();

                foreach (var day in weekDays)
                {
                    var dailySchedules = scheduleData.Where(sche => sche.startDate.Value.Date == day).ToList();
                    if (dailySchedules.Any())
                    {
                        foreach (var schedule in dailySchedules)
                        {
                            var empDetails = schedule.AuthScheEmps.Select(se => se.AuthEmp).ToList();
                            scheduleArray.Add(new object[] { day.ToString("dddd, dd MMMM yyyy"), schedule, empDetails });
                        }
                    }
                    else
                    {
                        scheduleArray.Add(new object[] { day.ToString("dddd, dd MMMM yyyy"), null, null });
                    }
                }

                return ErrorConst.Success("Lấy danh sách lịch hẹn thành công", scheduleArray);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        static List<DateTime> GetWeekDays(DateTime startDate, int offset = 0)
        {
            List<DateTime> weekDays = new List<DateTime>();
            DateTime startOfWeek = startDate.AddDays(-(int)startDate.DayOfWeek + (offset * 7));

            for (int i = 0; i < 7; i++)
            {
                weekDays.Add(startOfWeek.AddDays(i));
            }

            return weekDays;
        }
    }
}
