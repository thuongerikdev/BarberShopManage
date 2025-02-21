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
    public class AuthSpecService : AuthServiceBase , IAuthSpecService
    {
        public AuthSpecService(ILogger <AuthSpecService> logger , AuthDbContext authDbContext) : base(logger, authDbContext)
        {
        }
        public async Task <ResponeDto> AuthCreateSpec(AuthCreateSpecDto authCreateSpecDto)
        {
            _logger.LogInformation("AuthCreateSpec");
            try
            {
                var specialty = new AuthSpecialty
                {
                    specialtyName = authCreateSpecDto.specialtyName,
                    description = authCreateSpecDto.description,
                    note = authCreateSpecDto.note,
                    status = authCreateSpecDto.status,
                };
                _dbContext.Specialties.Add(specialty);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo chuyên môn thành công", specialty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateSpec(AuthUpdateSpecDto authUpdateSpecDto)
        {
            _logger.LogInformation("AuthUpdateSpec");
            try
            {
                var specialty = await _dbContext.Specialties.FindAsync(authUpdateSpecDto.specialtyID);
                if (specialty == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy chuyên môn");
                }
                specialty.specialtyName = authUpdateSpecDto.specialtyName;
                specialty.description = authUpdateSpecDto.description;
                specialty.note = authUpdateSpecDto.note;
                specialty.status = authUpdateSpecDto.status;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật chuyên môn thành công", specialty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteSpec(int specialtyID)
        {
            _logger.LogInformation("AuthDeleteSpec");
            try
            {
                var specialty = await _dbContext.Specialties.FindAsync(specialtyID);
                if (specialty == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy chuyên môn");
                }
                _dbContext.Specialties.Remove(specialty);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa chuyên môn thành công", specialty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetSpec(int specialtyID)
        {
            _logger.LogInformation("AuthGetSpec");
            try
            {
                var specialty = await _dbContext.Specialties.FindAsync(specialtyID);
                if (specialty == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy chuyên môn");
                }
                return ErrorConst.Success("Lấy thông tin chuyên môn thành công", specialty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllSpec()
        {
            _logger.LogInformation("AuthGetAllSpec");
            try
            {
                var specialties = await _dbContext.Specialties.ToListAsync();
                return ErrorConst.Success("Lấy danh sách chuyên môn thành công", specialties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
