using BM.Constant;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Social
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialBlogImageController : Controller
    {
        private readonly ISocialBlogImageService _socialBlogImageService;
        public SocialBlogImageController(ISocialBlogImageService socialBlogImageService)
        {
            _socialBlogImageService = socialBlogImageService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] SocialCreateBlogImageDto socialCreateBlogImageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _socialBlogImageService.SocialCreateBlogImage(socialCreateBlogImageDto);
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
        public async Task<IActionResult> Update([FromForm] SocialUpdateBlogImageDto socialUpdateBlogImageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _socialBlogImageService.SocialUpdateBlogImage(socialUpdateBlogImageDto);
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
        [HttpDelete("delete/{blogImageID}")]
        public async Task<IActionResult> Delete(int blogImageID)
        {
            if (blogImageID <= 0)
            {
                return BadRequest(ErrorConst.Error(500, "ID không hợp lệ"));
            }
            try
            {
                var result = await _socialBlogImageService.SocialDeleteBlogImage(blogImageID);
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
        [HttpGet("get/{blogImageID}")]
        public async Task<IActionResult> Get(int blogImageID)
        {
            if (blogImageID <= 0)
            {
                return BadRequest(ErrorConst.Error(500, "ID không hợp lệ"));
            }
            try
            {
                var result = await _socialBlogImageService.SocialGetBlogImage(blogImageID);
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
            try
            {
                var result = await _socialBlogImageService.SocialGetAllBlogImage();
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
