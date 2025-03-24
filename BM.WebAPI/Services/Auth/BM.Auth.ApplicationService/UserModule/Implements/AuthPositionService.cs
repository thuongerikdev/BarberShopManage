using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos.Position;
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
    public class AuthPositionService : AuthServiceBase, IAuthPositionService
    {
        public AuthPositionService (ILogger <AuthPositionService> logger , AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task <ResponeDto> AuthCreatePosition(AuthCreatePositionDto authCreatePositionDto)
        {
            _logger.LogInformation("AuthCreatePosition");
            try
            {
                var position = new AuthPosition
                {
                    positionName = authCreatePositionDto.positionName,
                    description = authCreatePositionDto.description,
                    note = authCreatePositionDto.note,
                    status = authCreatePositionDto.status,
                    DefaultSalary = authCreatePositionDto.DefaultSalary,

                };
                _dbContext.Positions.Add(position);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success( "tạo vị trí làm việc thành công" ,position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task <ResponeDto>  AuthUpdatePosition(AuthUpdatePositionDto authUpdatePositionDto)
        {
            _logger.LogInformation("AuthUpdatePosition");
            try
            {
                var position = await _dbContext.Positions.FindAsync(authUpdatePositionDto.positionID);
                if (position == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy vị trí làm việc");
                }
                position.positionName = authUpdatePositionDto.positionName;
                position.description = authUpdatePositionDto.description;
                position.note = authUpdatePositionDto.note;
                position.status = authUpdatePositionDto.status;
                position.DefaultSalary = authUpdatePositionDto.DefaultSalary;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật vị trí làm việc thành công", position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeletePosition(int positionID)
        {
            _logger.LogInformation("AuthDeletePosition");
            try
            {
                var position = await _dbContext.Positions.FindAsync(positionID);
                if (position == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy vị trí làm việc");
                }
                _dbContext.Positions.Remove(position);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa vị trí làm việc thành công", position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetPosition(int positionID)
        {
            _logger.LogInformation("AuthGetPosition");
            try
            {
                var position = await _dbContext.Positions.FindAsync(positionID);
                if (position == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy vị trí làm việc");
                }
                return ErrorConst.Success("Lấy thông tin vị trí làm việc thành công", position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllPosition()
        {
            _logger.LogInformation("AuthGetAllPosition");
            try
            {
                var positions = await _dbContext.Positions.ToListAsync();
                return ErrorConst.Success("Lấy danh sách vị trí làm việc thành công", positions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }

}
