using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Dtos.Position;
using BM.Auth.Dtos.User;
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
        public async Task<double> CaculateSalary(int positionID, int specialtyID)
        {
            var pos = await _authPositionService.AuthGetPosition(positionID);
            if (pos == null || pos.Data == null)
            {
                _logger.LogWarning($"Position with ID {positionID} not found.");
                return 0;
            }

            var posResult = pos.Data as AuthPosition;
            if (posResult == null)
            {
                _logger.LogWarning($"Position data for ID {positionID} is not of type AuthReadPositionDto.");
                return 0;
            }
            var salaryDefault = posResult.DefaultSalary;

            var spec = await _authSpecService.AuthGetSpec(specialtyID);
            if (spec == null || spec.Data == null)
            {
                _logger.LogWarning($"Specialty with ID {specialtyID} not found.");
                return 0;
            }
            var specResult = spec.Data as AuthSpecialty;
            if (specResult == null)
            {
                _logger.LogWarning($"Specialty data for ID {specialtyID} is not of type AuthReadSpecDto.");
                return 0;
            }
            var percent = specResult.percent;

            var salary = salaryDefault + (salaryDefault * percent / 100);
            return salary;
        }

        public async Task<ResponeDto> AuthCreateEmp(AuthCreateEmpDto authCreateEmpDto)
        {
            _logger.LogInformation("AuthCreateEmp");
            try
            {
                // Kiểm tra xem userID đã tồn tại trong bảng AuthEmp chưa (nếu có ràng buộc UNIQUE)
                var existingEmp = await _dbContext.Emps.FirstOrDefaultAsync(e => e.userID == authCreateEmpDto.userID);
                if (existingEmp != null)
                {
                    return ErrorConst.Error(400, $"User with ID {authCreateEmpDto.userID} is already associated with an employee (empID: {existingEmp.empID}).");
                }

                // Kiểm tra positionID và specialtyID
                var posCheck = await _authPositionService.AuthGetPosition(authCreateEmpDto.positionID);
                if (posCheck == null || posCheck.Data == null)
                {
                    return ErrorConst.Error(400, $"Position with ID {authCreateEmpDto.positionID} does not exist.");
                }

                var specCheck = await _authSpecService.AuthGetSpec(authCreateEmpDto.specialtyID);
                if (specCheck == null || specCheck.Data == null)
                {
                    return ErrorConst.Error(400, $"Specialty with ID {authCreateEmpDto.specialtyID} does not exist.");
                }

                var salary = await CaculateSalary(authCreateEmpDto.positionID, authCreateEmpDto.specialtyID);
                if (salary == 0)
                {
                    return ErrorConst.Error(500, "Unable to calculate salary due to invalid position or specialty data.");
                }

                var empCode = Guid.NewGuid().ToString();

                var emp = new AuthEmp
                {
                    empCode = empCode,
                    positionID = authCreateEmpDto.positionID,
                    specialtyID = authCreateEmpDto.specialtyID,
                    salary = salary,
                    startDate = DateTime.Now,
                    userID = authCreateEmpDto.userID,
                    status = "OK",
                    bonusSalary = 0,
                    branchID = authCreateEmpDto.branchID
                };
                _dbContext.Emps.Add(emp);
                await _dbContext.SaveChangesAsync();

                // Ánh xạ sang DTO
                var pos = await _authPositionService.AuthGetPosition(emp.positionID);
                var empDto = new AuthEmpDto
                {
                    empID = emp.empID,
                    empCode = emp.empCode,
                    positionID = emp.positionID,
                    positionName = pos?.Data is AuthReadPositionDto posDto ? posDto.positionName : null,
                    specialtyID = emp.specialtyID,
                    salary = emp.salary,
                    startDate = emp.startDate,
                    userID = emp.userID,
                    status = emp.status,
                    branchID = emp.branchID,
                    bonusSalary = emp.bonusSalary
                };

                return ErrorConst.Success("Tạo nhân viên thành công", empDto);
            }
            catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx && sqlEx.Number == 2601)
            {
                return ErrorConst.Error(400, $"User with ID {authCreateEmpDto.userID} is already associated with an employee.");
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
        public async Task<ResponeDto> AuthGetAllUserEmp()
        {
            _logger.LogInformation("AuthGetAllEmp");
            try
            {
                var emps = await _dbContext.Users.Where(x => x.isEmp == true).ToListAsync();

                return ErrorConst.Success("Lấy danh sách nhân viên thành công", emps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetEmpByUserID(int userID)
        {
            _logger.LogInformation("AuthGetEmpByUserID");
            try
            {
                var emp = await _dbContext.Emps.Where(x=> x.userID == userID)
                    .Include(x => x.AuthPosition)
                    .Include(x => x.AuthSpecialty)
                    .Include(x => x.AuthBranches)
                    .Include(x => x.AuthUser)
                    .Include(x => x.AuthScheEmps)
                    .Select(userID => new AuthGetEmpDto
                    {
                        empID = userID.empID,
                        empCode = userID.empCode,
                        positionID = userID.positionID,
                        positionName = userID.AuthPosition.positionName,
                        specialtyID = userID.specialtyID,
                        salary = userID.salary,
                        startDate = userID.startDate,
                        userID = userID.userID,
                        status = userID.status,
                        branchID = userID.branchID,
                        bonusSalary = userID.bonusSalary,
                        branchName = userID.AuthBranches.branchName,
                        specialtyName = userID.AuthSpecialty.specialtyName,
                        image = userID.AuthUser.avatar,
                        email = userID.AuthUser.email,
                        phone = userID.AuthUser.phoneNumber,
                        dateOfBirth = userID.AuthUser.dateOfBirth,
                        gender = userID.AuthUser.gender,
                        fullName = userID.AuthUser.fullName,
                        branchesImage = userID.AuthBranches.branchImage



                    })


                    .FirstOrDefaultAsync();
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
        
    }
}
