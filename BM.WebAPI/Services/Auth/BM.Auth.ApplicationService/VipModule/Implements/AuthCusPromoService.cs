using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos.User;
using BM.Auth.Infrastructure;
using BM.Constant;
using BM.Constant.Dto;
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
    public class AuthCusPromoService : AuthServiceBase, IAuthCusPromoService  , IGetPromotionShared
    {
        public AuthCusPromoService(ILogger<AuthCusPromoService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreateCusPromo(AuthCreateCusPromo authCreateCusPromo)
        {
            _logger.LogInformation("AuthCreateCusPromo");
            try
            {
                var cusPromo = new Domain.AuthCusPromo
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
        public async Task<ResponeDto> GetCustomerAndPromotionAsync(int customerID, int promoID)
        {
            _logger.LogInformation("AuthGetCusPromoByCustomer");
            try
            {
                // Truy vấn AuthCusPromo với customerID và promoID, bao gồm AuthCustomer và AuthPromotion
                var cusPromo = await _dbContext.CusPromos
                    .Where(x => x.customerID == customerID && x.promoID == promoID)
                    .Include(x => x.AuthCustomer)
                    .Include(x => x.AuthPromotion)
                    .FirstOrDefaultAsync();

                // Kiểm tra nếu không tìm thấy bản ghi
                if (cusPromo == null)
                {
                    _logger.LogWarning($"No promotion found for customerID: {customerID} and promoID: {promoID}");
                    return ErrorConst.Error(404, "Không tìm thấy khuyến mãi cho khách hàng này.");
                }

                // Tạo DTO để trả về dữ liệu
                var result = new CustomerPromotionDto
                {
                    CustomerData = new CustomerDto
                    {
                        CustomerID = cusPromo.AuthCustomer.customerID,
                        UserID = cusPromo.AuthCustomer.userID,
                        VipID = cusPromo.AuthCustomer.vipID,
                        LoyaltyPoints = cusPromo.AuthCustomer.loyaltyPoints,
                        CustomerType = cusPromo.AuthCustomer.customerType,
                        CustomerStatus = cusPromo.AuthCustomer.customerStatus,
                        TotalSpent = cusPromo.AuthCustomer.totalSpent,
                        PercentDiscount = cusPromo.AuthCustomer.percentDiscount
                    },
                    PromotionData = new PromotionDto
                    {
                        PromoID = cusPromo.AuthPromotion.promoID,
                        PromoName = cusPromo.AuthPromotion.promoName,
                        PromoDescription = cusPromo.AuthPromotion.promoDescription,
                        PromoDiscount = cusPromo.AuthPromotion.promoDiscount,
                        PromoStart = cusPromo.AuthPromotion.promoStart,
                        PromoEnd = cusPromo.AuthPromotion.promoEnd,
                        PromoStatus = cusPromo.AuthPromotion.promoStatus
                    }
                };

                return ErrorConst.Success("Lấy thông tin khuyến mãi và khách hàng thành công", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving customer and promotion");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task <ResponeDto> GetCusPromoByCustomerID (int customerID)
        {
            _logger.LogInformation("AuthGetCusPromoByCustomer");
            try
            {
                var cusPromo = await _dbContext.CusPromos
                    .Where(x => x.customerID == customerID)
                    .Include(x => x.AuthCustomer)
                    .Include(x => x.AuthPromotion)
                    .Select(x => x.AuthPromotion )
                    .ToListAsync();
                if (cusPromo == null || !cusPromo.Any())
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi cho khách hàng này");
                }
                return ErrorConst.Success("Lấy khuyến mãi khách hàng thành công", cusPromo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
