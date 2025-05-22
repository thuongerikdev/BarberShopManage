using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Dtos;
using BM.Auth.Dtos.User;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Auth
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
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] int userId, [FromQuery] string token)
        {
            if (userId <= 0 || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(ErrorConst.Error(400, "UserID hoặc token không hợp lệ."));
            }

            try
            {
                var result = await _authUserService.VerifyEmail(userId, token);
                if (result.ErrorCode == 200)
                {
                    // Trả về phản hồi thành công, có thể kèm theo redirect hoặc thông báo
                    return Ok(result); // Hoặc redirect tới trang xác nhận thành công
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
            catch (Exception ex)
            {
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
                if (result.ErrorCode == 200)
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
        [HttpPost("registerEmp")]
        public async Task<IActionResult> RegisterEmp([FromBody] List<AuthRegisteEmprDto> authRegisterDto)
        {
            if (authRegisterDto == null)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {

                var result = await _authUserService.AuthRegisterEmp(authRegisterDto);
                if (result.ErrorCode == 200)
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
                if (result.ErrorCode == 200)
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
                if (result.ErrorCode == 200)
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
                if (result.ErrorCode == 200)
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
                if (result.ErrorCode == 200)
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
        [HttpPut("updateAvatar")]
        public async Task<IActionResult> updateAvatar([FromForm] AuthUpdateAvatarDto authUpdateAvatarDto)
        {
            try
            {
                var result = await _authUserService.AuthUpdateUserAvatar(authUpdateAvatarDto.userID, authUpdateAvatarDto.file);
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
        [HttpPut("updateFullName")]
        public async Task<IActionResult> updateFullName(int userID, string FullName)
        {
            try
            {
                var result = await _authUserService.AuthChangeUserFullName(userID, FullName);
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
        [HttpPut("updateBasicUserInfo")]
        public async Task<IActionResult> updateBasicUserInfo([FromBody] UpdateBasicUserInfoDto updateBasicUserInfoDto)
        {
            try
            {
                var result = await _authUserService.AuthUpdateBasicUserInfor(updateBasicUserInfoDto);
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
