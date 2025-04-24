using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Booking
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingCategoryController : Controller
    {
        protected readonly IBookingCategoryService _bookingCategoryService;
        public BookingCategoryController(IBookingCategoryService bookingCategoryService)
        {
            _bookingCategoryService = bookingCategoryService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] BookingCreateCategoryDto bookingCreateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingCategoryService.BookingCreateCategory(bookingCreateCategoryDto);
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
        public async Task<IActionResult> Update([FromForm] BookingUpdateCategoryDto bookingUpdateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingCategoryService.BookingUpdateCategory(bookingUpdateCategoryDto);
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
        [HttpDelete("delete/{categoryID}")]
        public async Task<IActionResult> Delete(int categoryID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingCategoryService.BookingDeleteCategory(categoryID);
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
                var result = await _bookingCategoryService.BookingGetAllCategory();
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

        [HttpGet("get/{categoryID}")]
        public async Task<IActionResult> Get(int categoryID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingCategoryService.BookingGetCategory(categoryID);
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
