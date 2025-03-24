using BM.Auth.Dtos;
using BM.Booking.ApplicationService.BussinessModule.Abtracts;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
using BM.Booking.Dtos.BussinessDtos;
using BM.Booking.Dtos.CRUDdtos;
using BM.Booking.Dtos.CRUDDtos;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.Bussiness
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingBussinessController : Controller
    {
        private readonly IBookingBussinessService _bookingBussinessService;
        protected readonly IBookingInvoiceService _bookingInvoiceService;
        protected readonly IBookingOrderService _bookingOrderService;

        public BookingBussinessController(
            IBookingBussinessService bookingBussinessService,
            IBookingInvoiceService invoiceService,
            IBookingOrderService bookingOrderService

            )
        {
            _bookingBussinessService = bookingBussinessService;
            _bookingInvoiceService = invoiceService;
            _bookingOrderService = bookingOrderService;
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
            var totalMoneyString = Request.Query["vnp_Amount"]; // Lấy giá trị Amount dưới dạng StringValues

            // Chuyển đổi giá trị Amount từ StringValues sang decimal
            double totalAmount;
            if (!double.TryParse(totalMoneyString, out totalAmount))
            {
                return BadRequest(new { message = "Invalid total amount", code = "400" });
            }

            var orderID = ExtractTournamentIdFromOrderInfo(vnpOrderInfo);

            var response = _bookingBussinessService.PaymentExecute(Request.Query);

            //return Json(new { orderID });

            var invoice = new BookingCreateInvoiceDto
            {
                orderID = orderID,
                paymentTerms = "**Điều khoản thanh toán:**\r\n\r\n1. **Thời gian thanh toán:** Hóa đơn này phải được thanh toán trong vòng 30 ngày kể từ ngày lập hóa đơn (Invoice Date). Nếu không nhận được thanh toán trong thời gian quy định, chúng tôi có quyền áp dụng lãi suất 1.5% mỗi tháng trên số tiền chưa thanh toán.\r\n\r\n2. **Phương thức thanh toán:** Khách hàng có thể thực hiện thanh toán qua các phương thức sau:\r\n   - Chuyển khoản ngân hàng trực tiếp vào tài khoản ngân hàng của chúng tôi.\r\n   - Thanh toán bằng thẻ tín dụng thông qua hệ thống thanh toán trực tuyến của chúng tôi.\r\n   - Tiền mặt tại văn phòng của chúng tôi trong giờ làm việc.\r\n\r\n3. **Chi phí phát sinh:** Tất cả các chi phí phát sinh liên quan đến việc thu hồi khoản nợ sẽ do khách hàng chịu. Điều này bao gồm nhưng không giới hạn ở chi phí pháp lý, phí thu hồi nợ, và các chi phí khác phát sinh trong quá trình thu hồi.\r\n\r\n4. **Khách hàng có trách nhiệm:** Khách hàng có trách nhiệm kiểm tra thông tin trên hóa đơn và thông báo ngay cho chúng tôi nếu có bất kỳ sự không chính xác nào trong vòng 7 ngày kể từ ngày nhận hóa đơn.\r\n\r\n5. **Điều kiện hủy bỏ:** Nếu khách hàng không thanh toán đúng hạn, chúng tôi có quyền hủy bỏ hoặc tạm dừng tất cả các dịch vụ đang cung cấp cho khách hàng cho đến khi khoản nợ được thanh toán đầy đủ.",
                status = "Paid",
                totalAmount = totalAmount,
                invoiceDate = DateTime.Now,
                paymentMethod = "VnPay"

            };
            //var invoiceCreate = await _bookingInvoiceService.BookingCreateInvoice(invoice);

            if (response.VnPayResponseCode == "00")
            {
                //await _emailService.SendInvoiceEmail(tournamentId);
                await _bookingInvoiceService.BookingCreateInvoice(invoice);
                await _bookingOrderService.BookingConfirmOrder(orderID);

                return Json(new { response, orderID });
            }

            return BadRequest(new { message = "Payment failed", code = response.VnPayResponseCode });
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
