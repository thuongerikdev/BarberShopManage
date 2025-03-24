using BM.Booking.ApplicationService.PaymentModule.Abtracts;
using BM.Booking.Dtos.CRUDDtos;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Booking
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingInvoiceController : Controller
    {
        private readonly IBookingInvoiceService _bookingInvoiceService;
        public BookingInvoiceController(IBookingInvoiceService bookingInvoiceService)
        {
            _bookingInvoiceService = bookingInvoiceService;
        }
        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice([FromBody] BookingCreateInvoiceDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }

            try
            {
                var result = await _bookingInvoiceService.BookingCreateInvoice(request);
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
        [HttpPost("UpdateInvoice")]
        public async Task<IActionResult> UpdateInvoice([FromBody] BookingUpdateInvoiceDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }

            try
            {
                var result = await _bookingInvoiceService.BookingUpdateInvoice(request);
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
        [HttpDelete("DeleteInvoice/{invoiceID}")]
        public async Task<IActionResult> DeleteInvoice(int invoiceID)
        {
            try
            {
                var result = await _bookingInvoiceService.BookingDeleteInvoice(invoiceID);
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
        [HttpGet("GetInvoice/{invoiceID}")]
        public async Task<IActionResult> GetInvoice(int invoiceID)
        {
            try
            {
                var result = await _bookingInvoiceService.BookingGetInvoice(invoiceID);
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
        [HttpGet("GetAllInvoice")]
        public async Task<IActionResult> GetAllInvoice()
        {
            try
            {
                var result = await _bookingInvoiceService.BookingGetAllInvoice();
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

    }
}
