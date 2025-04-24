using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Dtos.User;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthVipController : Controller
    {
        private readonly IAuthVipService _authVipService;
        public AuthVipController(IAuthVipService authVipService)
        {
            _authVipService = authVipService;
        }
        [HttpPost("createvip")]
        public async Task<IActionResult> CreateVip([FromForm] AuthCreateVip authCreateVip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authVipService.AuthCreateVip(authCreateVip);
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
        [HttpPut("updatevip")]
        public async Task<IActionResult> UpdateVip([FromForm] AuthUpdateVip authUpdateVip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authVipService.AuthUpdateVip(authUpdateVip);
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
        [HttpDelete("deletevip/{vipID}")]
        public async Task<IActionResult> DeleteVip(int vipID)
        {
            if (vipID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "VipID không hợp lệ."));
            }
            try
            {
                var result = await _authVipService.AuthDeleteVip(vipID);
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
        [HttpGet("getvip/{vipID}")]
        public async Task<IActionResult> GetVip(int vipID)
        {
            if (vipID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "VipID không hợp lệ."));
            }
            try
            {
                var result = await _authVipService.AuthGetVip(vipID);
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
        [HttpGet("getallvip")]
        public async Task<IActionResult> GetAllVip()
        {
            try
            {
                var result = await _authVipService.AuthGetAllVip();
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
