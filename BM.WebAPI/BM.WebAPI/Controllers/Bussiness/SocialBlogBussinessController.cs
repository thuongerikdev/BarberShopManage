using BM.Constant;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.Bussiness
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialBlogBussinessController : Controller
    {
        private readonly ISocialBussiness _socialBussiness;

        public SocialBlogBussinessController(ISocialBussiness socialBussiness)
        {
            _socialBussiness = socialBussiness;
        }

        [HttpGet("create")] // Route: api/SocialBlogBussiness/create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("createBlog")]
        public async Task<IActionResult> Create([FromForm] CreateBlogRequestDto request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ErrorConst.Error(400, "Dữ liệu đầu vào không hợp lệ"));
            }

            try
            {
                var sections = request.Topics.Select((t, i) => (topic: t, content: request.Contents[i], image: i < request.Images.Count ? request.Images[i] : null)).ToList();
                var blogResponse = await _socialBussiness.CreateBlogAsync(request.BlogTitle, request.BlogStatus, sections);

                if (blogResponse.ErrorCode != 0) // Sửa từ 200 thành 0
                {
                    return BadRequest(blogResponse);
                }

                return Ok(blogResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorConst.Error(500, $"Lỗi hệ thống: {ex.Message}"));
            }
        }

        [HttpGet("detail/{id}")] // Route: api/SocialBlogBussiness/detail/{id}
        public async Task<IActionResult> Detail(int id)
        {
            var blog = await _socialBussiness.GetBlogAsync(id);
            if (blog == null) return NotFound();
            return Ok(blog);
        }
    }
}