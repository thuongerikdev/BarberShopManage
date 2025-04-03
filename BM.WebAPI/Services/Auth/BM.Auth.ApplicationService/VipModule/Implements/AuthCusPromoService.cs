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
    public class AuthCusPromoService : AuthServiceBase, IAuthCusPromoService
    {
        public AuthCusPromoService(ILogger<AuthCusPromoService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreateCusPromo(AuthCreateCusPromo authCreateCusPromo)
        {
            _logger.LogInformation("AuthCreateCusPromo");
            try
            {
                var cusPromo = new AuthCusPromo
                {
                    customerID = authCreateCusPromo.customerID,
                    promoID = authCreateCusPromo.promoID,
                    cusPromoStatus = authCreateCusPromo.cusPromoStatus
                };
                _dbContext.CusPromos.Add(cusPromo);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo khuyến mãi khách hàng thành công", cusPromo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateCusPromo(AuthUpdateCusPromo authUpdateCusPromo)
        {
            _logger.LogInformation("AuthUpdateCusPromo");
            try
            {
                var cusPromo = await _dbContext.CusPromos.FindAsync(authUpdateCusPromo.cusPromoID);
                if (cusPromo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi khách hàng");
                }
                cusPromo.customerID = authUpdateCusPromo.customerID;
                cusPromo.promoID = authUpdateCusPromo.promoID;
                cusPromo.cusPromoStatus = authUpdateCusPromo.cusPromoStatus;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật khuyến mãi khách hàng thành công", cusPromo);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteCusPromo(int cusPromoID)
        {
            _logger.LogInformation("AuthDeleteCusPromo");
            try
            {
                var cusPromo = await _dbContext.CusPromos.FindAsync(cusPromoID);
                if (cusPromo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi khách hàng");
                }
                _dbContext.CusPromos.Remove(cusPromo);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa khuyến mãi khách hàng thành công", cusPromo);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            
            }
        }


        public async Task<ResponeDto> AuthGetCusPromo(int cusPromoID)
        {
            _logger.LogInformation("AuthGetCusPromo");
            try
            {
                var cusPromo = await _dbContext.CusPromos.FindAsync(cusPromoID);
                if (cusPromo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi khách hàng");
                }
                return ErrorConst.Success("Lấy khuyến mãi khách hàng thành công", cusPromo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> AuthGetAllCusPromo()
        {
            _logger.LogInformation("AuthGetAllCusPromo");
            try
            {
                var cusPromos = _dbContext.CusPromos.ToList();
                return ErrorConst.Success("Lấy danh sách khuyến mãi khách hàng thành công", cusPromos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }


        }
    }
}
