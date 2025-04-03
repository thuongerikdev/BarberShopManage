using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Dtos.User;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthBranchController : Controller
    {
        private readonly IAuthBranchService _authBranchService;
        public AuthBranchController(IAuthBranchService authBranchService)
        {
            _authBranchService = authBranchService;
        }
        [HttpPost("createbranch")]
        public async Task<IActionResult> CreateBranch([FromBody] AuthCreateBranch authCreateBranch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authBranchService.AuthCreateBranch(authCreateBranch);
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
        [HttpPut("updatebranch")]
        public async Task<IActionResult> UpdateBranch([FromBody] AuthUpdateBranch authUpdateBranch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authBranchService.AuthUpdateBranch(authUpdateBranch);
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
        [HttpDelete("deletebranch/{branchID}")]
        public async Task<IActionResult> DeleteBranch(int branchID)
        {
            if (branchID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "BranchID không hợp lệ"));
            }
            try
            {
                var result = await _authBranchService.AuthDeleteBranch(branchID);
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
        [HttpGet("getbranch/{branchID}")]
        public async Task<IActionResult> GetBranch(int branchID)
        {
            if (branchID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "BranchID không hợp lệ"));
            }
            try
            {
                var result = await _authBranchService.AuthGetBranch(branchID);
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
        [HttpGet("getallbranch")]
        public async Task<IActionResult> GetAllBranch()
        {
            try
            {
                var result = await _authBranchService.AuthGetAllBranch();
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
        [HttpGet("getempByBranch/{branchID}")]
        public async Task<IActionResult> GetEmpByBranch(int branchID)
        {
            if (branchID <= 0)
            {
                return BadRequest(ErrorConst.Error(400, "BranchID không hợp lệ"));
            }
            try
            {
                var result = await _authBranchService.AuthGetEmpByBranch(branchID);
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
