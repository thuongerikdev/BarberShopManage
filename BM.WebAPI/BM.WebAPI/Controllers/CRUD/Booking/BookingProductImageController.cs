using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingProductImageController : Controller
    {
        private readonly IBookingProductImageService _bookingProductImageService;
        public BookingProductImageController(IBookingProductImageService bookingProductImageService)
        {
            _bookingProductImageService = bookingProductImageService;
        }
        [HttpPost("CreateProductImage")]
        public async Task<IActionResult> CreateProductImage([FromBody] BookingCreateProductImageDto bookingCreateProductInageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingProductImageService.BookingCreateProductImage(bookingCreateProductInageDto);
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
        [HttpPut("UpdateProductImage")]
        public async Task<IActionResult> UpdateProductImage([FromBody] BookingUpdateProductImageDto bookingUpdateProductImageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingProductImageService.BookingUpdateProductImage(bookingUpdateProductImageDto);
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
        [HttpGet("GetProductImage/{productImageID}")]
        public async Task<IActionResult> GetProductImage(int productImageID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingProductImageService.BookingGetProductImage(productImageID);
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
        [HttpDelete("DeleteProductImage/{productImageID}")]
        public async Task<IActionResult> DeleteProductImage(int productImageID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingProductImageService.BookingDeleteProductImage(productImageID);
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
        [HttpGet("GetAllProductImage")]
        public async Task<IActionResult> GetAllProductImage()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingProductImageService.BookingGetAllProductImage();
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
