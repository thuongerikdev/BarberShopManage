using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
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

namespace BM.Auth.ApplicationService.UserModule.Implements
{
    public class AuthCustomerService : AuthServiceBase, IAuthCustomerService, IGetCustomerByOrUserID
    {
        public AuthCustomerService(ILogger<AuthCustomerService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
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
                    percentDiscount = 0,
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
        public async Task <ResponeDto> AuthUpdateVipCustomer(AuthUpdateVipCustomerDto authUpdateVipCustomerDto)
        {
            try
            {
                var customer = await _dbContext.Customers.FindAsync(authUpdateVipCustomerDto.customerID);
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }
                var vipCustomer = await _dbContext.Vips.FindAsync(customer.vipID);
                if (vipCustomer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng vip");
                }
                customer.totalSpent += authUpdateVipCustomerDto.totalAmount;
                customer.loyaltyPoints += authUpdateVipCustomerDto.totalAmount *10/100 ;
                var totalLoyaltyPoints = customer.loyaltyPoints;
                if (totalLoyaltyPoints > vipCustomer.vipCost)
                {
                    customer.vipID += 1;
                }
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật khách hàng vip thành công", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
