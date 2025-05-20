using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Dtos.User;
using BM.Auth.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BM.Auth.ApplicationService.VipModule.Implements
{
    public class AuthCustomerCheckInService : AuthServiceBase , IAuthCustomerCheckInService
    {
        public AuthCustomerCheckInService(ILogger<AuthCustomerCheckInService> logger, AuthDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> AuthCreateCustomerCheckIn(AuthCreateCustomerCheckIn authCreateCustomerCheckIn)
        {
            _logger.LogInformation("AuthCreateCustomerCheckIn for customerID: {CustomerID}", authCreateCustomerCheckIn.customerID);
            try
            {
                // Kiểm tra khách hàng
                var customer = await _dbContext.Customers.FindAsync(authCreateCustomerCheckIn.customerID);
                if (customer == null)
                {
                    _logger.LogWarning("Customer not found for customerID: {CustomerID}", authCreateCustomerCheckIn.customerID);
                    return ErrorConst.Error(404, "Không tìm thấy khách hàng");
                }

                // Sử dụng múi giờ Việt Nam (UTC+7)
                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone).Date;

                // Kiểm tra xem khách hàng đã điểm danh hôm nay chưa
                var existingCheckIn = await _dbContext.customerCheckIns
                    .AnyAsync(x => x.customerID == authCreateCustomerCheckIn.customerID
                                && x.checkInDate.Date == today);
                if (existingCheckIn)
                {
                    _logger.LogWarning("Customer {CustomerID} already checked in today", authCreateCustomerCheckIn.customerID);
                    return ErrorConst.Error(400, "Khách hàng đã điểm danh hôm nay");
                }

                // Đếm số ngày điểm danh trong tháng hiện tại
                var monthStart = new DateTime(today.Year, today.Month, 1);
                var daysChecked = await _dbContext.customerCheckIns
                    .Where(x => x.customerID == authCreateCustomerCheckIn.customerID
                                && x.checkInDate >= monthStart
                                && x.checkInDate < monthStart.AddMonths(1))
                    .Select(x => x.checkInDate.Date)
                    .Distinct()
                    .CountAsync();

                // Tính điểm thưởng dựa trên số ngày điểm danh trong tháng hiện tại
                int pointsToAdd = daysChecked switch
                {
                    0 => 50,        // Lần đầu điểm danh trong tháng
                    < 5 => 100,     // Dưới 5 ngày
                    < 10 => 150,    // Dưới 10 ngày
                    < 20 => 200,    // Dưới 20 ngày
                    < 25 => 250,    // Dưới 25 ngày
                    _ => 300        // Từ 25 ngày trở lên
                };

                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // Cập nhật điểm thưởng
                    customer.loyaltyPoints += pointsToAdd;

                    // Tạo bản ghi điểm danh
                    var customerCheckIn = new AuthCustomerCheckIn
                    {
                        customerID = authCreateCustomerCheckIn.customerID,
                        checkInStatus = authCreateCustomerCheckIn.checkInStatus,
                        checkInDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone),
                        createAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone),
                        checkInType = authCreateCustomerCheckIn.checkInType,
                        updateAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone)
                    };
                    _dbContext.customerCheckIns.Add(customerCheckIn);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Tạo DTO cho response
                    var responseData = new CustomerCheckInResponseDto
                    {
                        CustomerId = customerCheckIn.customerID,
                        CheckInStatus = customerCheckIn.checkInStatus,
                        CheckInDate = customerCheckIn.checkInDate,
                        CheckInType = customerCheckIn.checkInType,
                        CreateAt = customerCheckIn.createAt,
                        UpdateAt = customerCheckIn.updateAt,
                        PointsAdded = pointsToAdd,
                        TotalLoyaltyPoints = customer.loyaltyPoints
                    };

                    _logger.LogInformation("Check-in successful for customerID: {CustomerID}, Points added: {PointsAdded}",
                        authCreateCustomerCheckIn.customerID, pointsToAdd);
                    return ErrorConst.Success("Điểm danh thành công", responseData);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error during check-in for customerID: {CustomerID}", authCreateCustomerCheckIn.customerID);
                    return ErrorConst.Error(500, "Lỗi khi thực hiện điểm danh");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during AuthCreateCustomerCheckIn for customerID: {CustomerID}",
                    authCreateCustomerCheckIn.customerID);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdateCustomerCheckIn(AuthUpdateCustomerCheckIn authUpdateCustomerCheckIn)
        {
            _logger.LogInformation("AuthUpdateCustomerCheckIn");
            try
            {
                var customerCheckIn = await _dbContext.customerCheckIns.FindAsync(authUpdateCustomerCheckIn.customerCheckInID);
                if (customerCheckIn == null)
                {
                    return ErrorConst.Error(500, "khong tim thay customer check in");
                }
                customerCheckIn.customerID = authUpdateCustomerCheckIn.customerID;
                customerCheckIn.checkInStatus = authUpdateCustomerCheckIn.checkInStatus;
                customerCheckIn.checkInType = authUpdateCustomerCheckIn.checkInType;
                customerCheckIn.updateAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cap nhat customer check in thanh cong", customerCheckIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeleteCustomerCheckIn(int customerCheckInID)
        {
            _logger.LogInformation("AuthDeleteCustomerCheckIn");
            try
            {
                var customerCheckIn = await _dbContext.customerCheckIns.FindAsync(customerCheckInID);
                if (customerCheckIn == null)
                {
                    return ErrorConst.Error(500, "khong tim thay customer check in");
                }
                _dbContext.customerCheckIns.Remove(customerCheckIn);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("xoa customer check in thanh cong", customerCheckIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetCustomerCheckIn(int customerCheckInID)
        {
            _logger.LogInformation("AuthGetCustomerCheckIn");
            try
            {
                var customerCheckIn = await _dbContext.customerCheckIns.FindAsync(customerCheckInID);
                if (customerCheckIn == null)
                {
                    return ErrorConst.Error(500, "khong tim thay customer check in");
                }
                return ErrorConst.Success("lay customer check in thanh cong", customerCheckIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllCustomerCheckIn()
        {
            _logger.LogInformation("AuthGetAllCustomerCheckIn");
            try
            {
                var customerCheckIns = await _dbContext.customerCheckIns.ToListAsync();
                if (customerCheckIns == null)
                {
                    return ErrorConst.Error(500, "khong tim thay customer check in");
                }
                return ErrorConst.Success("lay customer check in thanh cong", customerCheckIns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetCustomerCheckInByCustomerID(int customerID)
        {
            _logger.LogInformation("AuthGetCustomerCheckInByCustomerID");
            try
            {
                var customerCheckIns = await _dbContext.customerCheckIns.Where(x => x.customerID == customerID).ToListAsync();
                if (customerCheckIns == null)
                {
                    return ErrorConst.Error(500, "khong tim thay customer check in");
                }
                return ErrorConst.Success("lay customer check in thanh cong", customerCheckIns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
