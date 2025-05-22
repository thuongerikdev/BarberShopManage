using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Dtos.User;
using BM.Auth.Infrastructure;
using BM.Constant;
using BM.Shared.ApplicationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.UserModule.Implements
{
    public class AuthCustomerService : AuthServiceBase, IAuthCustomerService, IGetCustomerByOrUserID
    {
        protected readonly IAuthCusPromoService _cusPromoService;
        public AuthCustomerService(ILogger<AuthCustomerService> logger, AuthDbContext dbContext , IAuthCusPromoService cusPromoService) : base(logger, dbContext)
        {
            _cusPromoService = cusPromoService;
        }
        public async Task<ResponeDto> AuthCreateCustomer(AuthCreateCustomerDto authCreateCustomerDto)
        {
            _logger.LogInformation("AuthCreateCustomer");
            try
            {
                var customer = new AuthCustomer
                {
                    customerStatus = authCreateCustomerDto.customerStatus,
                    customerType = authCreateCustomerDto.customerType,
                    userID = authCreateCustomerDto.userID,
                    vipID = authCreateCustomerDto.vipID,
                    totalSpent = 0,
                    loyaltyPoints = 0,

                };
                _dbContext.Customers.Add(customer);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo khách hàng thành công", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateCustomer(AuthUpdateCustomerDto authUpdateCustomerDto)
        {
            _logger.LogInformation("AuthUpdateCustomer");
            try
            {
                var customer = await _dbContext.Customers.FindAsync(authUpdateCustomerDto.customerID);
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }
                customer.customerStatus = authUpdateCustomerDto.customerStatus;
                customer.customerType = authUpdateCustomerDto.customerType;

                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật khách hàng thành công", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteCustomer(int customerID)
        {
            _logger.LogInformation("AuthDeleteCustomer");
            try
            {
                var customer = await _dbContext.Customers.FindAsync(customerID);
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }
                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa khách hàng thành công", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetCustomer(int customerID)
        {
            _logger.LogInformation("AuthGetCustomer");
            try
            {
                var customer = await _dbContext.Customers.FindAsync(customerID);
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }
                return ErrorConst.Success("Lấy thông tin khách hàng thành công", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllCustomer()
        {
            _logger.LogInformation("AuthGetAllCustomer");
            try
            {
                var customers = await _dbContext.Customers.ToListAsync();
                return ErrorConst.Success("Lấy danh sách khách hàng thành công", customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> GetCustomerByOrUserID(int userID)
        {
            _logger.LogInformation("GetCustomerByOrUserID");
            try
            {
                var customer = await _dbContext.Customers.Where(x => x.userID == userID).FirstOrDefaultAsync();
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }
                return ErrorConst.Success("Lấy thông tin khách hàng thành công", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task <ResponeDto> AuthGetCustomerByUserID(int userID)
        {
            _logger.LogInformation("AuthGetCustomerbyUserID");
            try
            {
                var customer = await _dbContext.Customers.Where(x => x.userID == userID).FirstOrDefaultAsync();
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }
                return ErrorConst.Success("Lấy thông tin khách hàng thành công", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateVipCustomer(AuthUpdateVipCustomerDto authUpdateVipCustomerDto)
        {
            try
            {
                var customer = await _dbContext.Customers.FindAsync(authUpdateVipCustomerDto.customerID);
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }

                var currentVip = await _dbContext.Vips.FindAsync(customer.vipID);
                if (currentVip == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy thông tin VIP hiện tại");
                }

                // Update customer spending and loyalty points
                customer.totalSpent += authUpdateVipCustomerDto.totalAmount;
                customer.loyaltyPoints += (authUpdateVipCustomerDto.totalAmount * 10) / 1000;

                // Define VIP type progression
                string[] vipTypes = { "Vip 1", "Vip 2", "Vip 3", "Vip 4", "Vip 5" };
                int currentVipIndex = Array.IndexOf(vipTypes, currentVip.vipType);

                // Check if customer can upgrade to next VIP level
                if (currentVipIndex < vipTypes.Length - 1)
                {
                    string nextVipType = vipTypes[currentVipIndex + 1];
                    var nextVip = await _dbContext.Vips
                        .FirstOrDefaultAsync(v => v.vipType == nextVipType);

                    if (nextVip != null && customer.totalSpent >= nextVip.vipCost)
                    {
                        customer.vipID = nextVip.vipID;
                        //var promoDescription = $"Chúc mừng bạn trở thành {nextVip.vipType}";

                        var promo = await _dbContext.Promos
                            .Where(x => x.promoDescription.Contains(nextVip.vipType) && x.promoType == "VipPromotion")
                            .FirstOrDefaultAsync();
                        

                        if (promo != null)
                        {
                            var cusPromo = new AuthCreateCusPromo
                            {
                                customerID = customer.customerID,
                                promoID = promo.promoID,
                                cusPromoStatus = "OK",
                                promoCount = 2
                            };
                            await _cusPromoService.AuthCustomerGetVipPromo(cusPromo);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();

                // Map to DTO
                var responseDto = new CustomerVipResponseDto
                {
                    CustomerID = customer.customerID,
                    TotalSpent = customer.totalSpent,
                    LoyaltyPoints = customer.loyaltyPoints,
                    VipID = customer.vipID,
                    VipType = currentVip.vipType
                };

                return ErrorConst.Success("Cập nhật khách hàng VIP thành công", responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
