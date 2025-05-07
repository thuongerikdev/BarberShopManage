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



        [HttpPost("createBlog")] // Route: api/SocialBlogBussiness/createBlog
        public async Task<IActionResult> Create([FromForm] SocicalCreateBlogBussiness socicalCreateBlogBussiness)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(400, "Invalid input data."));
            }
            // Validate the input data
            try
            {
                if (socicalCreateBlogBussiness == null)
                {

                    return BadRequest(ErrorConst.Error(400, "Invalid blog data provided."));
                }

                var result = await _socialBussiness.CreateBlogAsync(socicalCreateBlogBussiness);
                if (result.ErrorCode != 200)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
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