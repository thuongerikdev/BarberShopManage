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
    public class AuthVipService : AuthServiceBase, IAuthVipService
    {
        public AuthVipService(ILogger<AuthVipService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreateVip(AuthCreateVip authCreateVip)
        {
            _logger.LogInformation("AuthCreateVip");
            try
            {
                var vip = new AuthVip
                {
                    vipCost = authCreateVip.vipCost,
                    vipDiscount = authCreateVip.vipDiscount,
                    vipStatus = authCreateVip.vipStatus,
                    vipType = authCreateVip.vipType
                };
                _dbContext.Vips.Add(vip);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo vip thành công", vip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateVip(AuthUpdateVip authUpdateVip)
        {
            _logger.LogInformation("AuthUpdateVip");
            try
            {
                var vip = await _dbContext.Vips.FindAsync(authUpdateVip.vipID);
                if (vip == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy vip");
                }
                vip.vipCost = authUpdateVip.vipCost;
                vip.vipDiscount = authUpdateVip.vipDiscount;
                vip.vipStatus = authUpdateVip.vipStatus;
                vip.vipType = authUpdateVip.vipType;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật vip thành công", vip);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteVip(int vipID)
        {
            _logger.LogInformation("AuthDeleteVip");
            try
            {
                var vip = await _dbContext.Vips.FindAsync(vipID);
                if (vip == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy vip");
                }
                _dbContext.Vips.Remove(vip);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa vip thành công", vip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetVip(int vipID)
        {
            _logger.LogInformation("AuthGetVip");
            try
            {
                var vip = await _dbContext.Vips.FindAsync(vipID);
                if (vip == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy vip");
                }
                return ErrorConst.Success("Lấy vip thành công", vip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllVip()
        {
            _logger.LogInformation("AuthGetAllVip");
            try
            {
                var vips = _dbContext.Vips.ToList();
                return ErrorConst.Success("Lấy danh sách vip thành công", vips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}

