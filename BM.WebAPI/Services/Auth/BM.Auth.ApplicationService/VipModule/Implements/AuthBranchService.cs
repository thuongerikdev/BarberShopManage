using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos.User;
using BM.Auth.Infrastructure;
using BM.Constant;
using BM.Shared.ApplicationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.VipModule.Implements
{
    public class AuthBranchService : AuthServiceBase, IAuthBranchService
    {
        protected readonly ICloudinaryService _cloudinaryService;
        public AuthBranchService(ILogger<AuthBranchService> logger, AuthDbContext dbContext , ICloudinaryService cloudinaryService) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> AuthCreateBranch(AuthCreateBranch authCreateBranch)
        {
            _logger.LogInformation("AuthCreateBranch");
            try
            {
                var img = await _cloudinaryService.UploadImageAsync(authCreateBranch.branchImage);
                if (img == null)
                {
                    return ErrorConst.Error(500, "Tải ảnh lên thất bại");
                }
                var branch = new AuthBranches
                {
                    branchName = authCreateBranch.branchName,
                    branchType = authCreateBranch.branchType,
                    branchStatus = authCreateBranch.branchStatus,
                    branchArea = authCreateBranch.branchArea,
                    branchHotline = authCreateBranch.branchHotline,
                    startWork = authCreateBranch.startWork,
                    endWork = authCreateBranch.endWork,
                    location = authCreateBranch.location,
                    branchImage = img,
                };
                _dbContext.Branches.Add(branch);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo chi nhánh thành công", branch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateBranch(AuthUpdateBranch authUpdateBranch)
        {
            _logger.LogInformation("AuthUpdateBranch");
            try
            {
                var img = await _cloudinaryService.UploadImageAsync(authUpdateBranch.branchImage);
                if (img == null)
                {
                    return ErrorConst.Error(500, "Tải ảnh lên thất bại");
                }
                var branch = await _dbContext.Branches.FindAsync(authUpdateBranch.branchID);
                if (branch == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy chi nhánh");
                }
                branch.branchName = authUpdateBranch.branchName;
                branch.branchType = authUpdateBranch.branchType;
                branch.branchStatus = authUpdateBranch.branchStatus;
                branch.branchArea = authUpdateBranch.branchArea;
                branch.branchHotline = authUpdateBranch.branchHotline;
                branch.startWork = authUpdateBranch.startWork;
                branch.endWork = authUpdateBranch.endWork;
                branch.location = authUpdateBranch.location;
                branch.branchImage = img;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật chi nhánh thành công", branch);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteBranch(int branchID)
        {
            _logger.LogInformation("AuthDeleteBranch");
            try
            {
                var branch = await _dbContext.Branches.FindAsync(branchID);
                if (branch == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy chi nhánh");
                }
                _dbContext.Branches.Remove(branch);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa chi nhánh thành công", branch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetBranch(int branchID)
        {
            _logger.LogInformation("AuthGetBranch");
            try
            {
                var branch = await _dbContext.Branches.FindAsync(branchID);
                if (branch == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy chi nhánh");
                }
                return ErrorConst.Success("Lấy chi nhánh thành công", branch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllBranch()
        {
            _logger.LogInformation("AuthGetAllBranch");
            try
            {
                var branches = _dbContext.Branches.ToList();
                return ErrorConst.Success("Lấy danh sách chi nhánh thành công", branches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetEmpByBranch(int branchID)
        {
            _logger.LogInformation("AuthGetEmpByBranch");
            try
            {
                var emp = await _dbContext.Emps
                    .Where(x => x.branchID == branchID)
                    .Join(
                        _dbContext.Users, // Assuming AuthUsers is the table with user data
                        emp => emp.userID,
                        user => user.userID, // Adjust userID to match your column name in AuthUsers
                        (emp, user) => new
                        {
                            emp.empID,
                            emp.userID,
                            emp.positionID,
                            emp.specialtyID,
                            emp.empCode,
                            emp.salary,
                            emp.bonusSalary,
                            emp.rate,
                            emp.status,
                            emp.branchID,
                            emp.startDate,
                            AuthUser = new
                            {
                                user.userID,
                                user.fullName, // Adjust to match your column name (e.g., FullName, Name)
                                user.email // Optional: include other fields if needed
                            }
                        }
                    )
                    .ToListAsync();

                if (emp == null || !emp.Any())
                {
                    return ErrorConst.Error(500, "Không tìm thấy nhân viên");
                }

                return ErrorConst.Success("Lấy danh sách nhân viên theo chi nhánh thành công", emp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
