using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Dtos;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthScheEmpController : Controller
    {
        private readonly IAuthScheEmpService _authScheEmpService;
        public AuthScheEmpController(IAuthScheEmpService authScheEmpService)
        {
            _authScheEmpService = authScheEmpService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> AuthCreateScheEmp( List<AuthCreateScheEmpDto> authCreateScheEmpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authScheEmpService.AuthCreateScheEmp(authCreateScheEmpDto);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("update")]
        public async Task<IActionResult> AuthUpdateScheEmp([FromBody] AuthUpdateScheEmpDto authUpdateScheEmpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authScheEmpService.AuthUpdateScheEmp(authUpdateScheEmpDto);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete/{scheEmpID}")]
        public async Task<IActionResult> AuthDeleteScheEmp(int scheEmpID)
        {
            try
            {
                var result = await _authScheEmpService.AuthDeleteScheEmp(scheEmpID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get/{scheEmpID}")]
        public async Task<IActionResult> AuthGetScheEmp(int scheEmpID)
        {
            try
            {
                var result = await _authScheEmpService.AuthGetScheEmp(scheEmpID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getall")]
        public async Task<IActionResult> AuthGetAllScheEmp()
        {
            try
            {
                var result = await _authScheEmpService.AuthGetAllScheEmp();
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getScheEmpByEmpID/{empID}")]
        public async Task<IActionResult> AuthGetScheEmpByEmpID(int empID)
        {
            try
            {
                var result = await _authScheEmpService.AuthGetEmployeeSchedule(empID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
