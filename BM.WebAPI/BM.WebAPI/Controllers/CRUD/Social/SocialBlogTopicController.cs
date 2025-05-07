using BM.Constant;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Social
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialBlogTopicController : Controller
    {
        private readonly ISocialBlogTopicService _socialBlogTopicService;
        public SocialBlogTopicController(ISocialBlogTopicService socialBlogTopicService)
        {
            _socialBlogTopicService = socialBlogTopicService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SocialCreateBlogTopicDto socialCreateBlogTopicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _socialBlogTopicService.SocialCreateBlogTopic(socialCreateBlogTopicDto);
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
        public async Task<IActionResult> Update([FromBody] SocialUpdateBlogTopicDto socialUpdateBlogTopicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _socialBlogTopicService.SocialUpdateBlogTopic(socialUpdateBlogTopicDto);
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
        [HttpDelete("delete/{topicID}")]
        public async Task<IActionResult> Delete(int topicID)
        {
            if (topicID <= 0)
            {
                return BadRequest(ErrorConst.Error(500, "ID không hợp lệ"));
            }
            try
            {
                var result = await _socialBlogTopicService.SocialDeleteBlogTopic(topicID);
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
        [HttpGet("get/{topicID}")]
        public async Task<IActionResult> Get(int topicID)
        {
            if (topicID <= 0)
            {
                return BadRequest(ErrorConst.Error(500, "ID không hợp lệ"));
            }
            try
            {
                var result = await _socialBlogTopicService.SocialGetBlogTopic(topicID);
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
                var result = await _socialBlogTopicService.SocialGetAllBlogTopic();
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
        //[HttpGet("getbyblog/{blogID}")]
        //public async Task<IActionResult> GetByBlog(int blogID)
        //{
        //    if (blogID <= 0)
        //    {
        //        return BadRequest(ErrorConst.Error(500, "ID không hợp lệ"));
        //    }
        //    try
        //    {
        //        var result = await _socialBlogTopicService.SocialGetBlogTopicByBlog(blogID);
        //        if (result == null)
        //        {
        //            return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
        //        }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ErrorConst.Error(500, ex.Message));
        //    }
        //}
    }
}
