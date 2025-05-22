using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.Dtos.CRUDdtos;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingServiceDetailDescriptionController : Controller
    {
        private readonly IBookingServiceDetailDescriptionService _bookingServiceDetailDescriptionService;
        public BookingServiceDetailDescriptionController(IBookingServiceDetailDescriptionService bookingServiceDetailDescriptionService)
        {
            _bookingServiceDetailDescriptionService = bookingServiceDetailDescriptionService;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] BookingCreateServiceDetailDescriptionDto createBookingServiceDetailDescriptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingServiceDetailDescriptionService.BookingCreateServiceDetailDescription(createBookingServiceDetailDescriptionDto);
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
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] BookingUpdateServiceDetailDescriptionDto updateBookingServiceDetailDescriptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingServiceDetailDescriptionService.BookingUpdateServiceDetailDescription(updateBookingServiceDetailDescriptionDto);
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
        [HttpDelete("Delete/{serviceDetailDescriptionID}")]
        public async Task<IActionResult> Delete(int serviceDetailDescriptionID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingServiceDetailDescriptionService.BookingDeleteServiceDetailDescription(serviceDetailDescriptionID);
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingServiceDetailDescriptionService.BookingGetAllServiceDetailDescription();
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
        [HttpGet("GetById/{serviceDetailDescriptionID}")]
        public async Task<ActionResult> Get(int serviceDetailDescriptionID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingServiceDetailDescriptionService.BookingGetServiceDetailDescriptionByID(serviceDetailDescriptionID);
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
        [HttpGet("GetByServiceDetailID/{serviceDetailID}")]
        public async Task<IActionResult> GetByServiceDetailID(int serviceDetailID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _bookingServiceDetailDescriptionService.BookingGetServiceDetailDescriptionByServiceDetailID(serviceDetailID);
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
