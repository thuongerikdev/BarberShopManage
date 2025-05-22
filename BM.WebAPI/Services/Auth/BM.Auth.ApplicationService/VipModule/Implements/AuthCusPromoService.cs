using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Dtos.User;
using BM.Auth.Infrastructure;
using BM.Constant;
using BM.Constant.Dto;
using BM.Shared.ApplicationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.VipModule.Implements
{
    public class AuthCusPromoService : AuthServiceBase, IAuthCusPromoService, IGetPromotionShared
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
                    cusPromoStatus = authCreateCusPromo.cusPromoStatus,
                    createAt = DateTime.Now,
                    promoCount = authCreateCusPromo.promoCount,
                    updateAt = DateTime.Now

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
                cusPromo.promoCount = authUpdateCusPromo.promoCount;
                cusPromo.updateAt = DateTime.Now;

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
                    },
                    promoCount = cusPromo.promoCount,
                    cusPromoID = cusPromo.cusPromoID,

                };

                return ErrorConst.Success("Lấy thông tin khuyến mãi và khách hàng thành công", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving customer and promotion");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> GetCusPromoByCustomerID(int customerID)
        {
            _logger.LogInformation("AuthGetCusPromoByCustomer");
            try
            {
                var cusPromo = await _dbContext.CusPromos
                    .Where(x => x.customerID == customerID)
                    .Include(x => x.AuthCustomer)
                    .Include(x => x.AuthPromotion)
                    .Select(x => new CuspromoDataGet
                    {
                        cusPromoID = x.cusPromoID ,
                        customerID = x.customerID,
                        promoID = x.promoID,
                        cusPromoStatus = x.cusPromoStatus,
                        promoCount = x.promoCount,
                        promoName = x.AuthPromotion.promoName,
                        promoDescription = x.AuthPromotion.promoDescription,
                        promoDiscount = x.AuthPromotion.promoDiscount,
                        pointToGet = x.AuthPromotion.pointToGet,
                        promoStart = x.AuthPromotion.promoStart,
                        promoEnd = x.AuthPromotion.promoEnd,
                        promoStatus = x.AuthPromotion.promoStatus,
                        promoType = x.AuthPromotion.promoType,
                        promoImage = x.AuthPromotion.promoImage,
                        userID = x.AuthCustomer.userID,
                        vipID = x.AuthCustomer.vipID,
                        loyaltyPoints = x.AuthCustomer.loyaltyPoints,
                    })
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
        public async Task<ResponeDto> AuthDecreasePromotion(int cuspromoID)
        {
            _logger.LogInformation("AuthDecreasePromotion");
            try
            {
                var cusPromo = await _dbContext.CusPromos.FindAsync(cuspromoID);
                if (cusPromo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi khách hàng");
                }
                if (cusPromo.promoCount > 0)
                {
                    cusPromo.promoCount--;
                    await _dbContext.SaveChangesAsync();
                    return ErrorConst.Success("Giảm số lượng khuyến mãi thành công", cusPromo);
                }
                else
                {
                    return ErrorConst.Error(500, "Số lượng khuyến mãi đã hết");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }

        }
        public async Task<ResponeDto> AuthIncreasePromotion(int cuspromoID)
        {
            _logger.LogInformation("AuthIncreasePromotion");
            try
            {
                var cusPromo = await _dbContext.CusPromos.FindAsync(cuspromoID);
                if (cusPromo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi khách hàng");
                }
                cusPromo.promoCount++;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tăng số lượng khuyến mãi thành công", cusPromo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthCustomerGetPromo(AuthCreateCusPromo authCreateCusPromo)
        {
            _logger.LogInformation("AuthCustomerGetPromo");
            try
            {
                // Kiểm tra khách hàng
                var customer = await _dbContext.Customers
                    .Where(x => x.customerID == authCreateCusPromo.customerID)
                    .FirstOrDefaultAsync();
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }

                // Kiểm tra khuyến mãi
                var promo = await _dbContext.Promos
                    .Where(x => x.promoID == authCreateCusPromo.promoID)
                    .FirstOrDefaultAsync();
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi");
                }

                // Kiểm tra nếu promoType là VipPromotion
                if (promo.promoType == "VipPromotion")
                {
                    return ErrorConst.Error(500, "Khuyến mãi VIP không được phép đổi");
                }

                var cusPromo = await _dbContext.CusPromos
                    .Where(x => x.customerID == authCreateCusPromo.customerID && x.promoID == authCreateCusPromo.promoID)
                    .FirstOrDefaultAsync();
                if (cusPromo != null)
                {
                    cusPromo.promoCount++;
                    await _dbContext.SaveChangesAsync();

                    var cusPromoResponse = new CusPromoResponseDto
                    {
                        CustomerID = cusPromo.customerID,
                        PromoID = cusPromo.promoID,
                        CusPromoStatus = cusPromo.cusPromoStatus,
                        PromoCount = cusPromo.promoCount,
                        CreateAt = cusPromo.createAt,
                        UpdateAt = cusPromo.updateAt
                    };
                    return ErrorConst.Success("Khách hàng đã nhận khuyến mãi thành công", cusPromoResponse);
                }

                if (customer.loyaltyPoints < promo.pointToGet)
                {
                    return ErrorConst.Error(500, "Điểm thưởng không đủ để nhận khuyến mãi");
                }

                // Sử dụng giao dịch để đảm bảo tính toàn vẹn
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // Thêm bản ghi CusPromos
                    var cusPromocreate = new Domain.AuthCusPromo
                    {
                        customerID = authCreateCusPromo.customerID,
                        promoID = authCreateCusPromo.promoID,
                        cusPromoStatus = authCreateCusPromo.cusPromoStatus,
                        createAt = DateTime.Now,
                        promoCount = 1,
                        updateAt = DateTime.Now
                    };
                    _dbContext.CusPromos.Add(cusPromocreate);

                    // Cập nhật điểm thưởng
                    customer.loyaltyPoints -= promo.pointToGet;

                    // Lưu tất cả thay đổi trong một lần
                    await _dbContext.SaveChangesAsync();

                    // Commit giao dịch
                    await transaction.CommitAsync();

                    var cusPromoResponse = new CusPromoResponseDto
                    {
                        CustomerID = cusPromocreate.customerID,
                        PromoID = cusPromocreate.promoID,
                        CusPromoStatus = cusPromocreate.cusPromoStatus,
                        PromoCount = cusPromocreate.promoCount,
                        CreateAt = cusPromocreate.createAt,
                        UpdateAt = cusPromocreate.updateAt
                    };
                    return ErrorConst.Success("Khách hàng đã nhận khuyến mãi thành công", cusPromoResponse);
                }
                catch (Exception ex)
                {
                    // Rollback giao dịch nếu có lỗi
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi cấp khuyến mãi");
                    return ErrorConst.Error(500, ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý AuthCustomerGetPromo");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthCustomerGetVipPromo(AuthCreateCusPromo authCreateCusPromo)
        {
            _logger.LogInformation("AuthCustomerGetPromo");
            try
            {

                // Sử dụng giao dịch để đảm bảo tính toàn vẹn
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // Thêm bản ghi CusPromos
                    var cusPromocreate = new Domain.AuthCusPromo
                    {
                        customerID = authCreateCusPromo.customerID,
                        promoID = authCreateCusPromo.promoID,
                        cusPromoStatus = authCreateCusPromo.cusPromoStatus,
                        createAt = DateTime.Now,
                        promoCount = authCreateCusPromo.promoCount,
                        updateAt = DateTime.Now
                    };
                    _dbContext.CusPromos.Add(cusPromocreate);



                    // Lưu tất cả thay đổi trong một lần
                    await _dbContext.SaveChangesAsync();

                    // Commit giao dịch
                    await transaction.CommitAsync();

                    var cusPromoResponse = new CusPromoResponseDto
                    {
                        CustomerID = cusPromocreate.customerID,
                        PromoID = cusPromocreate.promoID,
                        CusPromoStatus = cusPromocreate.cusPromoStatus,
                        PromoCount = cusPromocreate.promoCount,
                        CreateAt = cusPromocreate.createAt,
                        UpdateAt = cusPromocreate.updateAt
                    };
                    return ErrorConst.Success("Khách hàng đã nhận khuyến mãi thành công", cusPromoResponse);
                }
                catch (Exception ex)
                {
                    // Rollback giao dịch nếu có lỗi
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi cấp khuyến mãi");
                    return ErrorConst.Error(500, ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý AuthCustomerGetPromo");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthCustomerGetFreePromotion(int cusPromoID)
        {
            _logger.LogInformation("AuthGetPromoByCusPromoID");
            try
            {
                var cusPromo = await _dbContext.CusPromos
                    .Where(x => x.cusPromoID == cusPromoID)
                    .Include(x => x.AuthPromotion)
                    .FirstOrDefaultAsync();
                if (cusPromo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi khách hàng");
                }
                return ErrorConst.Success("Lấy thông tin khuyến mãi thành công", cusPromo.AuthPromotion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateCusPromoCount(int cusPromoID)
        {
            _logger.LogInformation("AuthGetPromoByCusPromoID");
            try
            {
                var cusPromo = await _dbContext.CusPromos.FindAsync(cusPromoID);
                if (cusPromo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi khách hàng");
                }
                cusPromo.promoCount++;
                return ErrorConst.Success("Lấy thông tin khuyến mãi thành công", cusPromo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> GetVipPromotion(string VipName)
        {
            _logger.LogInformation("GetVipPromotion");
            try
            {
                var promoDescription = "Chúc mừng bạn trở thành ${VipName}";

                var promo = await _dbContext.Promos
                    .Where(x => x.promoDescription.Contains(VipName) && x.promoType == "VipPromotion")
                    .FirstOrDefaultAsync();
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi VIP");
                }
                var promoData = new PromotionDto
                {
                    PromoID = promo.promoID,
                    PromoName = promo.promoName,
                    PromoDescription = promo.promoDescription,
                    PromoDiscount = promo.promoDiscount,
                    PromoStart = promo.promoStart,
                    PromoEnd = promo.promoEnd,
                    PromoStatus = promo.promoStatus
                };
                return ErrorConst.Success("Lấy thông tin khuyến mãi VIP thành công", promoData);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
