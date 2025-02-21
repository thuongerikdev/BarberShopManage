using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Dtos;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : Controller
    {
       private readonly IAuthUserService _authUserService;
        public AuthUserController(IAuthUserService authUserService)
        {
            _authUserService = authUserService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto authLoginDto)
        {
           if (!ModelState.IsValid)
           {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
           }
            try
            {
                var result = await _authUserService.AuthLogin(authLoginDto);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex) { 
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDto authRegisterDto)
        {
            if (authRegisterDto == null)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
               
                var result = await _authUserService.AuthRegister(authRegisterDto);
                if (result.ErrorCode==0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] AuthUpdateUserDto authUpdateUserDto)
        {
            if (authUpdateUserDto == null)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                
                var result = await _authUserService.AuthUpdateUser(authUpdateUserDto);
                if (result.ErrorCode == 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpDelete("delete/{userID}")]
        public async Task<IActionResult> Delete(int userID)
        {
            if (userID == 0)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authUserService.AuthDeleteUser(userID);
                if (result.ErrorCode == 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpGet("get/{userID}")]
        public async Task<IActionResult> Get(int userID)
        {
            if (userID == 0)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authUserService.AuthGetUser(userID);
                if (result.ErrorCode == 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _authUserService.AuthGetAllUser();
                if (result.ErrorCode == 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }

    }
}
