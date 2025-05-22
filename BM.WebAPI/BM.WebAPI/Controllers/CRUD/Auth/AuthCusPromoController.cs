using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Dtos.User;
using BM.Constant;
using BM.Shared.ApplicationService;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthCusPromoController : Controller
    {
        private IAuthCusPromoService _authCusPromoService;
        private IGetPromotionShared _getPromotionShared;
        public AuthCusPromoController(IAuthCusPromoService authCusPromoService, IGetPromotionShared getPromotionShared)
        {
            _authCusPromoService = authCusPromoService;
            _getPromotionShared = getPromotionShared;
        }
        [HttpPost("createcuspromo")]
        public async Task<IActionResult> CreateCusPromo([FromBody] AuthCreateCusPromo authCreateCusPromo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authCusPromoService.AuthCreateCusPromo(authCreateCusPromo);
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
        [HttpPut("updatecuspromo")]
        public async Task<IActionResult> UpdateCusPromo([FromBody] AuthUpdateCusPromo authUpdateCusPromo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authCusPromoService.AuthUpdateCusPromo(authUpdateCusPromo);
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
        [HttpDelete("deletecuspromo/{cusPromoID}")]
        public async Task<IActionResult> DeleteCusPromo(int cusPromoID)
        {
            if (cusPromoID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "cusPromoID không hợp lệ"));
            }
            try
            {
                var result = await _authCusPromoService.AuthDeleteCusPromo(cusPromoID);
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
        [HttpGet("getcuspromo/{cusPromoID}")]
        public async Task<IActionResult> GetCusPromo(int cusPromoID)
        {
            if (cusPromoID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "cusPromoID không hợp lệ"));
            }
            try
            {
                var result = await _authCusPromoService.AuthGetCusPromo(cusPromoID);
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
        [HttpGet("getallcuspromo")]
        public async Task<IActionResult> GetAllCusPromo()
        {
            try
            {
                var result = await _authCusPromoService.AuthGetAllCusPromo();
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
        [HttpGet("getcuspromobyCusAndProMo")]
        public async Task<IActionResult> GetCusPromoByCusAndProMo([FromQuery] int cusID, [FromQuery] int proMoID)
        {
            if (cusID <= 0 || proMoID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "cusID hoặc proMoID không hợp lệ"));
            }
            try
            {
                var result = await _getPromotionShared.GetCustomerAndPromotionAsync(cusID, proMoID);
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

        [HttpGet("getcuspromobycustomerid/{customerID}")]
        public async Task<IActionResult> GetCusPromoByCustomerID(int customerID)
        {
            if (customerID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "customerID không hợp lệ"));
            }
            try
            {
                var result = await _authCusPromoService.GetCusPromoByCustomerID(customerID);
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
        [HttpPost("customergetpromo")]
        public async Task<IActionResult> CustomerGetPromo([FromBody] AuthCreateCusPromo authCreateCusPromo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authCusPromoService.AuthCustomerGetPromo(authCreateCusPromo);
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
        [HttpPut("updateCuspromoCount/{cusPromoID}")]
        public async Task<IActionResult> UpdateCuspromoCount(int cusPromoID)
        {
            if (cusPromoID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "cusPromoID không hợp lệ"));
            }
            try
            {
                var result = await _authCusPromoService.AuthUpdateCusPromoCount(cusPromoID);
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
        [HttpGet("GetVipPromotion/{vipName}")]
        public async Task<IActionResult> GetVipPromotion(string vipName)
        {
            if (string.IsNullOrEmpty(vipName))
            {
                return BadRequest(ErrorConst.Error(400, "vipName không hợp lệ"));
            }
            try
            {
                var result = await _authCusPromoService.GetVipPromotion(vipName);
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
