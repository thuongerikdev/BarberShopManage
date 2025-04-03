using BM.Constant;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Social
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialSrcController : Controller
    {
        private readonly ISocialSrcService _srcService;

        public SocialSrcController(ISocialSrcService srcService)
        {
            _srcService = srcService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] SocialCreateSrcDto socialCreateSrcDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _srcService.SocialCreateSrc(socialCreateSrcDto);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "Thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SocialUpdateSrcDto socialUpdateSrcDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _srcService.SocialUpdateSrc(socialUpdateSrcDto);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "Thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }

        [HttpDelete("delete/{srcID}")]
        public async Task<IActionResult> Delete(int srcID)
        {
            try
            {
                var result = await _srcService.SocialDeleteSrc(srcID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "Thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }

        [HttpGet("get/{srcID}")]
        public async Task<IActionResult> Get(int srcID)
        {
            try
            {
                var result = await _srcService.SocialGetSrc(srcID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "Thông tin xác thực được cung cấp không chính xác"));
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
            try
            {
                var result = await _srcService.SocialGetAllSrc();
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "Thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }

        [HttpGet("getsrcbyname/{srcType}")]
        public async Task<IActionResult> GetSrcByName(string srcType)
        {
            try
            {
                var result = await _srcService.GetSocialSrcbyType(srcType);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "Thông tin xác thực được cung cấp không chính xác"));
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