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
    public class AuthEmpService : AuthServiceBase, IAuthEmpService
    {
        protected readonly IAuthSpecService _authSpecService;
        protected readonly IAuthPositionService _authPositionService;
        
        public AuthEmpService(ILogger<AuthEmpService> logger, AuthDbContext dbContext, IAuthSpecService authSpecService, IAuthPositionService authPositionService) : base(logger, dbContext)
        {
            _authSpecService = authSpecService;
            _authPositionService = authPositionService;
        }
        public async Task<double> CaculateSalary (int positionID, int specialtyID)
        {
            var pos = await _authPositionService.AuthGetPosition(positionID);
            if (pos == null)
            {
                return 0;
            }
            var posResult = pos.Data as AuthReadPositionDto;
            var salaryDefault = posResult.DefaultSalary;

            var spec = await _authSpecService.AuthGetSpec(specialtyID);
            if (spec == null)
            {
                return 0;
            }
            var specResult = spec.Data as AuthReadSpecDto;
            var percent = specResult.percent;

            var salary = salaryDefault + (salaryDefault * percent / 100);
            return salary;
        }


        public async Task<ResponeDto> AuthCreateEmp(AuthCreateEmpDto authCreateEmpDto)
        {
            _logger.LogInformation("AuthCreateEmp");
            try
            {
              

                var salary = await CaculateSalary(authCreateEmpDto.positionID , authCreateEmpDto.specialtyID);

                var empCode = Guid.NewGuid().ToString();

                var emp = new AuthEmp
                {
                    empCode = empCode,
                    positionID = authCreateEmpDto.positionID,
                    specialtyID = authCreateEmpDto.specialtyID,
                    salary = salary,
                    startDate = DateTime.Now,
                    userID = authCreateEmpDto.userID,


                };
                _dbContext.Emps.Add(emp);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo nhân viên thành công", emp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateEmp(AuthUpdateEmpDto authUpdateEmpDto)
        {
            _logger.LogInformation("AuthUpdateEmp");
            try
            {
                var emp = await _dbContext.Emps.FindAsync(authUpdateEmpDto.empID);
                if (emp == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy nhân viên");
                }
             
                emp.positionID = authUpdateEmpDto.positionID;
                emp.specialtyID = authUpdateEmpDto.specialtyID;

                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật nhân viên thành công", emp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteEmp(int empID)
        {
            _logger.LogInformation("AuthDeleteEmp");
            try
            {
                var emp = await _dbContext.Emps.FindAsync(empID);
                if (emp == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy nhân viên");
                }
                _dbContext.Emps.Remove(emp);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa nhân viên thành công", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetEmp(int empID)
        {
            _logger.LogInformation("AuthGetEmp");
            try
            {
                var emp = await _dbContext.Emps.FindAsync(empID);
                if (emp == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy nhân viên");
                }
                return ErrorConst.Success("Lấy thông tin nhân viên thành công", emp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllEmp()
        {
            _logger.LogInformation("AuthGetAllEmp");
            try
            {
                var emps = await _dbContext.Emps.ToListAsync();
                return ErrorConst.Success("Lấy danh sách nhân viên thành công", emps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    } 
}
