using BM.Auth.ApplicationService.BusinessModule.Abtracts;
using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Infrastructure;
using BM.Constant;
using BM.Constant.Dto;
using BM.Shared.ApplicationService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.BusinessModule.Implements
{
    public  class AuthBussinessService : AuthServiceBase , IAuthBussinessService 
    {
        private readonly ISendOrderData _sendOrderData;
        private readonly IAuthEmpService _authEmpService;

        public AuthBussinessService(
            ILogger <AuthBussinessService> logger , 
            AuthDbContext dbContext,
            ISendOrderData sendOrderData,
            IAuthEmpService authEmpService


            ) : base (logger , dbContext)
        {
            _sendOrderData = sendOrderData;
            _authEmpService = authEmpService;
        }
        public async Task<ResponeDto> AuthFinshedPayment(int orderID)
        {
            _logger.LogInformation("AuthFinshedPayment");
            try
            {
                // Lấy thông tin đơn hàng từ domain khác
                var sendedResponse = await _sendOrderData.GetOrderToOtherDomain(orderID);
                if (sendedResponse == null || sendedResponse.Data == null)
                {
                    return ErrorConst.Error(500, "Không thể lấy thông tin đơn hàng từ domain khác");
                }

                var sender = sendedResponse.Data as BookingGetCustAndEmpByOrderDto;
                if (sender == null)
                {
                    return ErrorConst.Error(500, "Dữ liệu đơn hàng không hợp lệ");
                }

                // Lấy thông tin khách hàng
                var customer = await _dbContext.Customers.FindAsync(sender.custID);
                if (customer == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khách hàng");
                }

                // Tính tổng tiền từ empMoney (giả sử totalMoney không có trong DTO, cần tính)
                double totalMoney = sender.empMoney?.Sum(e => e.money) ?? 0;
                if (totalMoney <= 0)
                {
                    return ErrorConst.Error(500, "Tổng tiền không hợp lệ");
                }

                // Cập nhật totalSpent
                customer.totalSpent += totalMoney;

                // Cấu hình VIP levels với điểm thưởng và chiết khấu
                var vipLevels = new Dictionary<string, (int PointsMultiplier, double Discount)>
                    {
                        { "VIP0", (1, 0.0) },
                        { "VIP1", (2, 0.1) },
                        { "VIP2", (3, 0.2) },
                        { "VIP3", (4, 0.3) },
                        { "VIP4", (5, 0.4) },
                        { "VIP5", (6, 0.5) },
                        { "VIP6", (7, 0.6) },
                        { "VIP7", (8, 0.7) },
                        { "VIP8", (9, 0.8) },
                        { "VIP9", (10, 0.9) }
                    };

                // Tính điểm thưởng dựa trên customerType hiện tại
                if (vipLevels.TryGetValue(customer.customerType, out var level))
                {
                    customer.loyaltyPoints += (int)(totalMoney * level.PointsMultiplier);
                    customer.percentDiscount = level.Discount;
                }
                else
                {
                    // Mặc định cho trường hợp không xác định
                    customer.customerType = "VIP0";
                    customer.loyaltyPoints += (int)totalMoney;
                    customer.percentDiscount = 0;
                }

                // Cập nhật customerType dựa trên loyaltyPoints
                customer.customerType = customer.loyaltyPoints switch
                {
                    < 100000 => "VIP0",
                    < 200000 => "VIP1",
                    < 300000 => "VIP2",
                    < 400000 => "VIP3",
                    < 500000 => "VIP4",
                    < 600000 => "VIP5",
                    < 700000 => "VIP6",
                    < 800000 => "VIP7",
                    < 900000 => "VIP8",
                    _ => "VIP9"
                };


                // Lưu thay đổi vào database
                _dbContext.Customers.Update(customer);

                foreach (var item in sender.empMoney)
                {
                    var emp = await _dbContext.Emps.FindAsync(item.empID);
                    emp.bonusSalary += item.money*0.1;
                }

                //_authEmpService.Ca

                await _dbContext.SaveChangesAsync();

                // Trả về kết quả thành công
                return ErrorConst.Success("Cập nhật thông tin thanh toán thành công", new
                {
                    customer.customerID,
                    customer.customerType,
                    customer.loyaltyPoints,
                    customer.totalSpent,
                    customer.percentDiscount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, $"Lỗi khi xử lý thanh toán: {ex.Message}");
            }
        }
        //public async Task<ResponeDto> AuthGetSaleOfEmp()
        //{

        //}

    }
}
