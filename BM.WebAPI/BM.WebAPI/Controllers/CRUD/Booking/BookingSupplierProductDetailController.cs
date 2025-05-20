using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingSupplierProductDetailController : Controller
    {
        private readonly IBookingSupplierProductDetailService _bookingSupplierProductDetailService;
        public BookingSupplierProductDetailController(IBookingSupplierProductDetailService bookingSupplierProductDetailService)
        {
            _bookingSupplierProductDetailService = bookingSupplierProductDetailService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BookingCreateSupplierProductDetailDto bookingCreateSupplierProductDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingSupplierProductDetailService.BookingCreateSupplierProductDetail(bookingCreateSupplierProductDetailDto);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BookingUpdateSupplierProductDetailDto bookingUpdateSupplierProductDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingSupplierProductDetailService.BookingUpdateSupplierProductDetail(bookingUpdateSupplierProductDetailDto);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpDelete("delete/{supplierProductDetailID}")]
        public async Task<IActionResult> Delete(int supplierProductDetailID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingSupplierProductDetailService.BookingDeleteSupplierProductDetail(supplierProductDetailID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpGet("get/{supplierProductDetailID}")]
        public async Task<IActionResult> Get(int supplierProductDetailID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingSupplierProductDetailService.BookingGetSupplierProductDetail(supplierProductDetailID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingSupplierProductDetailService.BookingGetAllSupplierProductDetail();
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
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
