using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Dtos;
using BM.Booking.ApplicationService.BussinessModule.Abtracts;
using BM.Booking.ApplicationService.BussinessModule.Implements;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
using BM.Booking.Domain;
using BM.Booking.Dtos.BussinessDtos;
using BM.Booking.Dtos.CRUDdtos;
using BM.Booking.Dtos.CRUDDtos;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;
using BookingOrder = BM.Booking.Domain.BookingOrder;

namespace BM.WebAPI.Controllers.Bussiness
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingBussinessController : Controller
    {
        private readonly IBookingBussinessService _bookingBussinessService;
        protected readonly IBookingInvoiceService _bookingInvoiceService;
        protected readonly IBookingOrderService _bookingOrderService;
        private readonly IAuthCustomerService _authCustomerService;


        public BookingBussinessController(
            IBookingBussinessService bookingBussinessService,
            IBookingInvoiceService invoiceService,
            IBookingOrderService bookingOrderService,
            IAuthCustomerService authCustomerService

            )
        {
            _bookingBussinessService = bookingBussinessService;
            _bookingInvoiceService = invoiceService;
            _bookingOrderService = bookingOrderService;
            _authCustomerService = authCustomerService;
        }
        //public async Task<IActionResult> CreateOrderAppoint([FromBody] BookingCreateOrderRequestDto request)

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrderAppoint([FromBody] BookingCreateOrderRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }

            try
            {
                var result = await _bookingBussinessService.CreateOrderAppoint(request.Appoint, request.Order, request.PromoID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "Thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpPost("vnpay/payment")]
        public async Task<IActionResult> CreatePaymentUrlVnpay(int orderID)
        {

            var url = _bookingBussinessService.CreatePaymentUrl(orderID, HttpContext);

            return Ok(new { PayUrl = url });
        }

        [HttpGet("vnpay/PaymentExecute")]

        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var vnpOrderInfo = Request.Query["vnp_OrderInfo"].ToString(); // Lấy OrderInfo
            var totalMoneyString = Request.Query["vnp_Amount"]; // Lấy giá trị Amount

            // Chuyển đổi giá trị Amount từ StringValues sang decimal
            double totalAmount;
            if (!double.TryParse(totalMoneyString, out totalAmount))
            {
                return Content(@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Payment Error</title>
                    <style>
                        body { font-family: Arial, sans-serif; text-align: center; padding: 50px; }
                        .error { color: red; }
                    </style>
                </head>
                <body>
                    <h1>Payment Failed</h1>
                    <p class='error'>Invalid total amount</p>
                    <p>Error Code: 400</p>
                    <a href='/'>Return to Home</a>
                </body>
                </html>", "text/html");
            }

            var orderID = ExtractTournamentIdFromOrderInfo(vnpOrderInfo);
            var order = await _bookingOrderService.BookingGetOrder(orderID);
            if (order == null)
            {
                return Content(@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Payment Error</title>
                    <style>
                        body { font-family: Arial, sans-serif; text-align: center; padding: 50px; }
                        .error { color: red; }
                    </style>
                </head>
                <body>
                    <h1>Payment Failed</h1>
                    <p class='error'>Order not found</p>
                    <p>Error Code: 404</p>
                    <a href='/'>Return to Home</a>
                </body>
                </html>", "text/html");
            }

            var orderData = order.Data as BookingOrder;
            var response = _bookingBussinessService.PaymentExecute(Request.Query);

            var invoice = new BookingCreateInvoiceDto
            {
                orderID = orderID,
                paymentTerms = "**Điều khoản thanh toán:**\r\n\r\n1. **Thời gian thanh toán:** Hóa đơn này phải được thanh toán trong vòng 30 ngày kể từ ngày lập hóa đơn (Invoice Date). Nếu không nhận được thanh toán trong thời gian quy định, chúng tôi có quyền áp dụng lãi suất 1.5% mỗi tháng trên số tiền chưa thanh toán.\r\n\r\n2. **Phương thức thanh toán:** Khách hàng có thể thực hiện thanh toán qua các phương thức sau:\r\n   - Chuyển khoản ngân hàng trực tiếp vào tài khoản ngân hàng của chúng tôi.\r\n   - Thanh toán bằng thẻ tín dụng thông qua hệ thống thanh toán trực tuyến của chúng tôi.\r\n   - Tiền mặt tại văn phòng của chúng tôi trong giờ làm việc.\r\n\r\n3. **Chi phí phát sinh:** Tất cả các chi phí phát sinh liên quan đến việc thu hồi khoản nợ sẽ do khách hàng chịu. Điều này bao gồm nhưng không giới hạn ở chi phí pháp lý, phí thu hồi nợ, và các chi phí khác phát sinh trong quá trình thu hồi.\r\n\r\n4. **Khách hàng có trách nhiệm:** Khách hàng có trách nhiệm kiểm tra thông tin trên hóa đơn và thông báo ngay cho chúng tôi nếu có bất kỳ sự không chính xác nào trong vòng 7 ngày kể từ ngày nhận hóa đơn.\r\n\r\n5. **Điều kiện hủy bỏ:** Nếu khách hàng không thanh toán đúng hạn, chúng tôi có quyền hủy bỏ hoặc tạm dừng tất cả các dịch vụ đang cung cấp cho khách hàng cho đến khi khoản nợ được thanh toán đầy đủ.",
                status = "Paid",
                totalAmount = totalAmount,
                invoiceDate = DateTime.Now,
                paymentMethod = "VnPay"
            };

            var updateDto = new AuthUpdateVipCustomerDto
            {
                customerID = orderData.custID,
                totalAmount = totalAmount/100
            };

            if (response.VnPayResponseCode == "00")
            {
                await _bookingInvoiceService.BookingCreateInvoice(invoice);
                await _bookingOrderService.BookingConfirmOrder(orderID);
                await _authCustomerService.AuthUpdateVipCustomer(updateDto);

                return Content($@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Payment Success</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; text-align: center; padding: 50px; }}
                        .success {{ color: green; }}
                        .details {{ margin-top: 20px; text-align: left; display: inline-block; }}
                    </style>
                </head>
                <body>
                    <h1>Payment Successful</h1>
                    <p class='success'>Your payment has been processed successfully!</p>
                    <div class='details'>
                        <p><strong>Order ID:</strong> {orderID}</p>
                        <p><strong>Total Amount:</strong> {totalAmount:C}</p>
                        <p><strong>Payment Method:</strong> VnPay</p>
                        <p><strong>Invoice Date:</strong> {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
                    </div>
                    <p>An invoice has been sent to your email.</p>
                    <a href='/'>Return to Home</a>
                </body>
                </html>", "text/html");
            }

            return Content($@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Payment Error</title>
                <style>
                    body {{ font-family: Arial, sans-serif; text-align: center; padding: 50px; }}
                    .error {{ color: red; }}
                </style>
            </head>
            <body>
                <h1>Payment Failed</h1>
                <p class='error'>Payment processing failed</p>
                <p>Error Code: {response.VnPayResponseCode}</p>
                <a href='/'>Return to Home</a>
            </body>
            </html>", "text/html");
        }
        // Hàm hỗ trợ tách TournamentID
        private int ExtractTournamentIdFromOrderInfo(string orderInfo)
        {
            // Giả định orderInfo có format: "Thanh toán hóa đơn cho giải đấu {TournamentID}"
            var words = orderInfo.Split(' ');
            var OrderIdString = words.Last(); // Lấy phần cuối cùng chứa TournamentID

            if (int.TryParse(OrderIdString, out int orderID))
            {
                return orderID; // Thành công
            }

            throw new FormatException("Invalid order ID in OrderInfo");
        }



    }
}
