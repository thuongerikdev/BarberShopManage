using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos.User;
using BM.Auth.Infrastructure;
using BM.Constant;
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
        public AuthBranchService(ILogger<AuthBranchService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreateBranch(AuthCreateBranch authCreateBranch)
        {
            _logger.LogInformation("AuthCreateBranch");
            try
            {
                var branch = new AuthBranches
                {
                    branchName = authCreateBranch.branchName,
                    branchType = authCreateBranch.branchType,
                    branchStatus = authCreateBranch.branchStatus,
                    branchArea = authCreateBranch.branchArea,
                    branchHotline = authCreateBranch.branchHotline,
                    startWork = authCreateBranch.startWork,
                    endWork = authCreateBranch.endWork,
                    location = authCreateBranch.location
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
                var emp = _dbContext.Emps.Where(x => x.branchID == branchID).ToList();
                if (emp == null)
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
