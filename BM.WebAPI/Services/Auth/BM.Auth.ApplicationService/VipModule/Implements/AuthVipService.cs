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
    public class AuthVipService : AuthServiceBase, IAuthVipService
    {
        protected readonly ICloudinaryService _cloudinaryService;
        public AuthVipService(ILogger<AuthVipService> logger, AuthDbContext dbContext, ICloudinaryService cloudinaryService) : base(logger, dbContext)
        {

            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> AuthCreateVip(AuthCreateVip authCreateVip)
        {
            _logger.LogInformation("AuthCreateVip");
            try
            {

                var img = await _cloudinaryService.UploadImageAsync(authCreateVip.Image);
                if (img == null)
                {
                    return ErrorConst.Error(500, "Tải ảnh lên thất bại");
                }

                var vip = new AuthVip
                {
                    vipCost = authCreateVip.vipCost,
                    vipDiscount = authCreateVip.vipDiscount,
                    vipStatus = authCreateVip.vipStatus,
                    vipType = authCreateVip.vipType,
                    vipImage = img,
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
                var img = await _cloudinaryService.UploadImageAsync(authUpdateVip.Image);
                if (vip == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy vip");
                }
                vip.vipCost = authUpdateVip.vipCost;
                vip.vipDiscount = authUpdateVip.vipDiscount;
                vip.vipStatus = authUpdateVip.vipStatus;
                vip.vipType = authUpdateVip.vipType;
                vip.vipImage = img;
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
        public async Task<ResponeDto> AuthGetVipByUserID(int userID)
        {
            _logger.LogInformation("AuthGetVipByUserID");
            try
            {
                var vip = await _dbContext.Customers.Where(x => x.userID == userID)
                                                    .Include(x => x.AuthVip)
                                                    .Include(x => x.AuthUser)
                                                    .Select(x => new
                                                    {
                                                        x.totalSpent,
                                                        x.loyaltyPoints,
                                                        x.AuthVip.vipCost,
                                                        x.AuthVip.vipDiscount,
                                                        x.AuthVip.vipStatus,
                                                        x.AuthVip.vipType,
                                                        x.AuthVip.vipImage,
                                                        x.AuthUser.fullName,
                                                        x.AuthUser.avatar

                                                    }).FirstOrDefaultAsync();


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
        public async Task<ResponeDto> AuthGetVipByType(string vipType)
        {
            _logger.LogInformation("AuthGetVipByType");
            try
            {
                var vip = await _dbContext.Vips.Where(x => x.vipType == vipType).ToListAsync();
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
        //public async Task<ResponeDto> AuthUpdateType(string vipType, int userID)
        //{
        //    _logger.LogInformation("AuthGetVipByType");
        //    try
        //    {
        //        var vip = await _dbContext.Vips.Where(x => x.vipType == vipType).ToListAsync();
        //        if (vip == null)
        //        {
        //            return ErrorConst.Error(500, "Không tìm thấy vip");
        //        }
        //        return ErrorConst.Success("Lấy vip thành công", vip);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return ErrorConst.Error(500, ex.Message);
        //    }
        //}
    }
}

