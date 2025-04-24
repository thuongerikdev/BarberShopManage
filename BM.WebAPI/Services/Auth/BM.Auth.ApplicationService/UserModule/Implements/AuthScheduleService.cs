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
        public async Task<ResponeDto> AuthCreateSchedule(List<AuthCreateScheduleDto> scheduleDtos)
        {
            _logger.LogInformation("AuthCreateSchedule - batch");

            try
            {
                var schedules = scheduleDtos.Select(dto => new AuthSchedule
                {
                    scheduleName = dto.scheduleName,
                    description = dto.description,
                    note = dto.note,
                    startDate = dto.startDate,
                    endDate = dto.endDate,
                    status = dto.status,
                    percent = dto.percent,
                }).ToList();

                await _dbContext.Schedules.AddRangeAsync(schedules);
                await _dbContext.SaveChangesAsync();

                return ErrorConst.Success("Tạo nhiều lịch hẹn thành công", schedules);
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
        public async Task<ResponeDto> GetEmployeesByDateAndBranch (DateTime date, int branchesID)
        {
            _logger.LogInformation($"Fetching employees by date {date:yyyy-MM-dd} and branch ID {branchesID}");

            try
            {
                // Truy vấn danh sách nhân viên theo ngày và chi nhánh
                var employeeSchedules = await _dbContext.ScheEmps
                    .Where(se => se.startDate <= date
                                && se.endDate >= date
                                && se.status == "OK"
                                && se.AuthEmp.branchID == branchesID) // Lọc theo branchID
                    .Include(se => se.AuthEmp) // Tải các quan hệ liên quan
                        .ThenInclude(emp => emp.AuthUser)
                    .Include(se => se.AuthEmp)
                        .ThenInclude(emp => emp.AuthPosition)
                    .Include(se => se.AuthEmp)
                        .ThenInclude(emp => emp.AuthSpecialty)
                    .Include(se => se.AuthEmp)
                        .ThenInclude(emp => emp.AuthBranches)
                    .Include(se => se.AuthSchedule)
                    .Select(se => new
                    {
                        EmployeeId = se.AuthEmp.empID,
                        EmployeeCode = se.AuthEmp.empCode,
                        EmployeeName = se.AuthEmp.AuthUser != null ? se.AuthEmp.AuthUser.fullName : "N/A",
                        Position = se.AuthEmp.AuthPosition != null ? se.AuthEmp.AuthPosition.positionName : "N/A",
                        Specialty = se.AuthEmp.AuthSpecialty != null ? se.AuthEmp.AuthSpecialty.specialtyName : "N/A",
                        Branch = se.AuthEmp.AuthBranches != null ? se.AuthEmp.AuthBranches.branchName : "N/A",
                        ScheduleName = se.AuthSchedule != null ? se.AuthSchedule.scheduleName : "N/A",
                        StartTime = se.startDate,
                        EndTime = se.endDate
                    })
                    .ToListAsync();

                // Kiểm tra nếu không có dữ liệu
                if (!employeeSchedules.Any())
                {
                    return ErrorConst.Success("Không tìm thấy nhân viên phù hợp", new List<object>());
                }

                return ErrorConst.Success("Lấy danh sách lịch nhân viên thành công", employeeSchedules);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while fetching employee schedules");
                return ErrorConst.Error(500, "Lỗi cơ sở dữ liệu khi lấy danh sách nhân viên");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching employee schedules");
                return ErrorConst.Error(500, $"Lỗi không xác định: {ex.Message}");
            }
        }
    }
}
