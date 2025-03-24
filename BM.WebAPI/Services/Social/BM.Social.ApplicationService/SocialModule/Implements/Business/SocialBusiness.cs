using BM.Constant;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Domain;
using BM.Social.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;
using StackExchange.Redis; // Thêm thư viện Redis
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.SocialModule.Implements.Business
{
    public class SocialBusiness : ISocialBussiness
    {
        private readonly ISocialBlogService _socialBlogService;
        private readonly ISocialSrcService _socialSrcService;
        private readonly ISocialSrcBlogService _socialSrcBlogService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IDatabase _redisDb; // Thêm Redis database instance

        public SocialBusiness(
            ISocialBlogService socialBlogService,
            ISocialSrcService socialSrcService,
            ISocialSrcBlogService socialSrcBlogService,
            ICloudinaryService cloudinaryService,
            IConnectionMultiplexer redis) // Inject Redis connection
        {
            _socialBlogService = socialBlogService;
            _socialSrcService = socialSrcService;
            _socialSrcBlogService = socialSrcBlogService;
            _cloudinaryService = cloudinaryService;
            _redisDb = redis.GetDatabase(); // Lấy Redis database
        }

        public async Task<ResponeDto> CreateBlogAsync(string title, string status, List<(string topic, string content, IFormFile? image)> sections)
        {
            try
            {
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(status))
                {
                    return ErrorConst.Error(400, "Tiêu đề và trạng thái không được để trống");
                }

                var blogSections = sections.Select(s => new BlogContentSection
                {
                    Topic = s.topic,
                    Content = s.content
                }).ToList();

                var blogContentJson = JsonSerializer.Serialize(blogSections);

                var blogDto = new SocialCreateBlogDto
                {
                    blogTitle = title,
                    blogContent = blogContentJson,
                    blogStatus = status,
                    blogLike = 0,
                };

                var blogRespone = await _socialBlogService.SocialCreateBlog(blogDto);
                if (blogRespone.ErrorCode != 0)
                {
                    return ErrorConst.Error(blogRespone.ErrorCode, $"Không thể tạo blog: {blogRespone.ErrorMessager}");
                }

                var blog = blogRespone.Data as SocialBlog;
                if (blog == null)
                {
                    return ErrorConst.Error(500, "Dữ liệu blog trả về không hợp lệ");
                }
                int blogID = blog.blogID;

                for (int i = 0; i < sections.Count; i++)
                {
                    var section = sections[i];
                    if (section.image != null && section.image.Length > 0)
                    {
                        var socialSrcDto = new SocialCreateSrcDto
                        {
                            srcName = $"Blog {blogID} - Section {i + 1}",
                            imageSrc = section.image,
                        };

                        var srcResponse = await _socialSrcService.SocialCreateSrc(socialSrcDto);
                        if (srcResponse.ErrorCode != 0)
                        {
                            return ErrorConst.Error(srcResponse.ErrorCode, $"Không thể tạo src cho section {i + 1}: {srcResponse.ErrorMessager}");
                        }

                        var src = srcResponse.Data as SocialSrc;
                        if (src == null)
                        {
                            return ErrorConst.Error(500, "Dữ liệu src trả về không hợp lệ");
                        }
                        var srcId = src.srcID;

                        var srcBlogDto = new SocialCreateSrcBlogDto
                        {
                            blogID = blogID,
                            srcID = srcId,
                            position = i + 1,
                        };
                        await _socialSrcBlogService.SocialCreateSrcBlog(srcBlogDto);
                    }
                }

                // Chuyển đổi sang DTO để loại bỏ tham chiếu vòng
                var cacheDto = new ResponeCacheDto
                {
                    ErrorCode = blogRespone.ErrorCode,
                    ErrorMessager = blogRespone.ErrorMessager,
                    Data = new SocialBlogCacheDto
                    {
                        BlogID = blog.blogID,
                        BlogTitle = blog.blogTitle,
                        BlogContent = blog.blogContent,
                        BlogStatus = blog.blogStatus,
                        BlogLike = blog.blogLike,
                        CreatedDate = DateTime.Now
                    }
                };

                var blogJson = JsonSerializer.Serialize(cacheDto);
                await _redisDb.StringSetAsync($"blog:{blogID}", blogJson, TimeSpan.FromMinutes(30));

                return ErrorConst.Success("Tạo blog thành công", blogID);
            }
            catch (Exception ex)
            {
                return ErrorConst.Error(500, $"Lỗi hệ thống khi tạo blog: {ex.Message}");
            }
        }
        public async Task<ResponeDto> GetBlogAsync(int blogId)
        {
            string cacheKey = $"blog:{blogId}";
            var cachedBlog = await _redisDb.StringGetAsync(cacheKey);

            if (cachedBlog.HasValue)
            {
                var cachedDto = JsonSerializer.Deserialize<ResponeCacheDto>(cachedBlog);
                return new ResponeDto
                {
                    ErrorCode = cachedDto.ErrorCode,
                    ErrorMessager = cachedDto.ErrorMessager,
                    Data = cachedDto.Data // Trả về SocialBlogCacheDto
                };
            }

            var blogResponse = await _socialBlogService.SocialGetBlog(blogId);

            if (blogResponse.ErrorCode == 0)
            {
                var blog = blogResponse.Data as SocialBlog;
                if (blog != null)
                {
                    var cacheDto = new ResponeCacheDto
                    {
                        ErrorCode = blogResponse.ErrorCode,
                        ErrorMessager = blogResponse.ErrorMessager,
                        Data = new SocialBlogCacheDto
                        {
                            BlogID = blog.blogID,
                            BlogTitle = blog.blogTitle,
                            BlogContent = blog.blogContent,
                            BlogStatus = blog.blogStatus,
                            BlogLike = blog.blogLike,
                            CreatedDate = DateTime.Now
                        }
                    };

                    var blogJson = JsonSerializer.Serialize(cacheDto);
                    await _redisDb.StringSetAsync(cacheKey, blogJson, TimeSpan.FromMinutes(30));
                }
            }

            return blogResponse;
        }
    }
}