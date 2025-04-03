using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Dtos.Position;
using BM.Auth.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Implements
{
    public class AuthScheduleService : AuthServiceBase, IAuthScheduleService
    {
        public AuthScheduleService(ILogger<AuthScheduleService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreateSchedule(AuthCreateScheduleDto authCreateScheduleDto)
        {
            _logger.LogInformation("AuthCreateSchedule");
            try
            {
                var schedule = new AuthSchedule
                {
                    scheduleName = authCreateScheduleDto.scheduleName,
                    description = authCreateScheduleDto.description,
                    note = authCreateScheduleDto.note,
                    startDate = authCreateScheduleDto.startDate,
                    endDate = authCreateScheduleDto.endDate,
                    status = authCreateScheduleDto.status,
                    percent = authCreateScheduleDto.percent,
                };
                _dbContext.Schedules.Add(schedule);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo lịch hẹn thành công", schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateSchedule(AuthUpdateScheduleDto authUpdateScheduleDto)
        {
            _logger.LogInformation("AuthUpdateSchedule");
            try
            {
                var schedule = await _dbContext.Schedules.FindAsync(authUpdateScheduleDto.scheduleID);
                if (schedule == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy lịch hẹn");
                }
                schedule.scheduleName = authUpdateScheduleDto.scheduleName;
                schedule.description = authUpdateScheduleDto.description;
                schedule.note = authUpdateScheduleDto.note;
                schedule.startDate = authUpdateScheduleDto.startDate;
                schedule.endDate = authUpdateScheduleDto.endDate;
                schedule.status = authUpdateScheduleDto.status;
                schedule.percent = authUpdateScheduleDto.percent;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật lịch hẹn thành công", schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteSchedule(int scheduleID)
        {
            _logger.LogInformation("AuthDeleteSchedule");
            try
            {
                var schedule = await _dbContext.Schedules.FindAsync(scheduleID);
                if (schedule == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy lịch hẹn");
                }
                _dbContext.Schedules.Remove(schedule);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa lịch hẹn thành công", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetSchedule(int scheduleID)
        {
            _logger.LogInformation("AuthGetSchedule");
            try
            {
                var schedule = await _dbContext.Schedules.FindAsync(scheduleID);
                if (schedule == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy lịch hẹn");
                }
                return ErrorConst.Success("Lấy lịch hẹn thành công", schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllSchedule()
        {
            _logger.LogInformation("AuthGetAllSchedule");
            try
            {
                var schedules = await _dbContext.Schedules.ToListAsync();
                return ErrorConst.Success("Lấy danh sách lịch hẹn thành công", schedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetEmpByDate(DateTime date)
        {
            _logger.LogInformation("Auth get emp by date");
            try
            {
                var schedules = await _dbContext.ScheEmps
                    .Where(se => se.startDate <= date
                        && se.endDate >= date
                        && se.status == "OK" // Giả sử "Active" là trạng thái đang hoạt động
                       )
                    .Select(se => new
                    {
                        EmployeeId = se.AuthEmp.empID,
                        EmployeeCode = se.AuthEmp.empCode,
                        EmployeeName = se.AuthEmp.AuthUser != null ? se.AuthEmp.AuthUser.fullName : null, // Giả sử AuthUser có field FullName
                        Position = se.AuthEmp.AuthPosition != null ? se.AuthEmp.AuthPosition.positionName : null,
                        Specialty = se.AuthEmp.AuthSpecialty != null ? se.AuthEmp.AuthSpecialty.specialtyName : null,
                        Branch = se.AuthEmp.AuthBranches != null ? se.AuthEmp.AuthBranches.branchName : null,
                        ScheduleName = se.AuthSchedule.scheduleName,
                        StartTime = se.startDate,
                        EndTime = se.endDate
                    })
                    .ToListAsync();

                return ErrorConst.Success("Lấy danh sách lịch hẹn thành công", schedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
