using BM.Constant;
using BM.Shared.ApplicationService;
using BM.Social.ApplicationService.Common;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Domain;
using BM.Social.Dtos;
using BM.Social.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class SocialBusiness : SocialServiceBase, ISocialBussiness 
    {
        private readonly ISocialBlogService _socialBlogService;
        private readonly ISocialBlogContentService _socialBlogContentService;
        private readonly ISocialBlogImageService _socialBlogImageService;
        private readonly ISocialBlogTopicService _socialBlogTopicService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IDatabase _redisDb; // Thêm Redis database instance



        public SocialBusiness(
            ISocialBlogService socialBlogService,
            ICloudinaryService cloudinaryService,
            ISocialBlogImageService socialBlogImageService,
            ISocialBlogTopicService socialBlogTopicService,
            ISocialBlogContentService socialBlogContentService,
            ILogger<SocialBusiness> logger,
            SocialDbContext dbContext,
            IConnectionMultiplexer redis)  : base(logger, dbContext)// Inject Redis connection


        {
            _socialBlogService = socialBlogService;
            _socialBlogContentService = socialBlogContentService;
            _socialBlogImageService = socialBlogImageService;
            _socialBlogTopicService = socialBlogTopicService;
            _cloudinaryService = cloudinaryService;
            _redisDb = redis.GetDatabase(); // Lấy Redis database
        }

        public async Task<ResponeDto> CreateBlogAsync(SocicalCreateBlogBussiness socicalCreateBlogBussiness)
        {
            try
            {
                // Validate input
                if (socicalCreateBlogBussiness == null || socicalCreateBlogBussiness.blogID <= 0)
                {
                    return ErrorConst.Error(400, "Invalid blog data provided.");
                }

                // Begin transaction
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // Create blog topics
                    foreach (var topic in socicalCreateBlogBussiness.topics)
                    {
                        var blogTopicDto = new SocialCreateBlogTopicDto
                        {
                            blogID = socicalCreateBlogBussiness.blogID,
                            topicContent = topic.topicContent,
                            topicTitle = topic.topicTitle,
                            topicStatus = "OK", // Use bool, assuming database expects bool
                            position = topic.position
                        };

                        var topicResult = await _socialBlogTopicService.SocialCreateBlogTopic(blogTopicDto);
                        if (topicResult.ErrorCode != 200)
                        {
                            await transaction.RollbackAsync();
                            return topicResult; // Propagate error if topic creation fails
                        }
                    }

                    // Create blog content
                    foreach (var content in socicalCreateBlogBussiness.contents)
                    {
                        var blogContentDto = new SocialCreateBlogContentDto
                        {
                            blogID = socicalCreateBlogBussiness.blogID,
                            contentTitle = content.contentTitle,
                            content = content.content,
                            position = content.position,
                            status = "OK" // Use bool, assuming database expects bool
                        };

                        var contentResult = await _socialBlogContentService.SocialCreateBlogContent(blogContentDto);
                        if (contentResult.ErrorCode != 200)
                        {
                            await transaction.RollbackAsync();
                            return contentResult; // Propagate error if content creation fails
                        }
                    }

                    // Create blog images
                    var uploadedImageUrls = new List<string>();
                    try
                    {
                        foreach (var image in socicalCreateBlogBussiness.images)
                        {
                            var blogImageDto = new SocialCreateBlogImageDto
                            {
                                blogID = socicalCreateBlogBussiness.blogID,
                                srcImage = image.srcImage,
                                position = image.position,
                                status = "OK" // Use bool, assuming database expects bool
                            };

                            var imageResult = await _socialBlogImageService.SocialCreateBlogImage(blogImageDto);
                            if (imageResult.ErrorCode != 200)
                            {
                                await transaction.RollbackAsync();
                                foreach (var url in uploadedImageUrls)
                                {
                                    await _cloudinaryService.DeleteImageAsync(url);
                                }
                                return imageResult; // Propagate error if image creation fails
                            }

                            // Store uploaded image URL for potential cleanup
                            if (imageResult.Data is SocialBlogImage blogImage)
                            {
                                uploadedImageUrls.Add(blogImage.srcImage);
                            }
                        }

                        // Commit transaction
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        foreach (var url in uploadedImageUrls)
                        {
                            await _cloudinaryService.DeleteImageAsync(url);
                        }
                        return ErrorConst.Error(500, $"Failed to create blog images: {ex.Message}");
                    }

                    // Cache blog metadata in Redis (outside transaction)
                    var cacheKey = $"blog:{socicalCreateBlogBussiness.blogID}";
                    var cacheData = new
                    {
                        socicalCreateBlogBussiness.blogID,
                        Topics = socicalCreateBlogBussiness.topics.Select(t => new { t.topicTitle, t.topicContent, t.position }),
                        Contents = socicalCreateBlogBussiness.contents.Select(c => new { c.contentTitle, c.content, c.position }),
                        Images = socicalCreateBlogBussiness.images.Select(i => new { i.position })
                    };
                    await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(cacheData), TimeSpan.FromHours(1));

                    return ErrorConst.Success("Blog created successfully", socicalCreateBlogBussiness);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ErrorConst.Error(500, $"Failed to create blog: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return ErrorConst.Error(500, $"Failed to create blog: {ex.Message}");
            }
        }

        public async Task<ResponeDto> GetBlogAsync(int blogId)
        {
            try
            {
                if (blogId <= 0)
                {
                    return ErrorConst.Error(400, "ID blog không hợp lệ");
                }

                // Check Redis cache
                var cacheKey = $"blog:{blogId}";
                var cachedData = await _redisDb.StringGetAsync(cacheKey);
                if (!cachedData.IsNullOrEmpty)
                {
                    var cachedResponse = JsonSerializer.Deserialize<ResponeDto>(cachedData);
                    if (cachedResponse != null && cachedResponse.ErrorCode == 200)
                    {
                        // Optional: Check if the cached data is still valid by comparing updateAt
                        var cachedBlog = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(cachedResponse.Data));
                        var blogUpdateAt = await _dbContext.socialBlogs
                            .Where(x => x.blogID == blogId)
                            .Select(x => x.updateAt)
                            .FirstOrDefaultAsync();

                        // Extract updateAt from JsonElement
                        if (blogUpdateAt == default ||
                            (cachedBlog.TryGetProperty("updateAt", out var cachedUpdateAt) &&
                             cachedUpdateAt.GetString() == blogUpdateAt.ToString("O")))
                        {
                            return ErrorConst.Success("Lấy blog thành công từ cache", cachedResponse.Data);
                        }
                    }
                }

                // Retrieve blog from database
                var blog = await _dbContext.socialBlogs
                    .Where(x => x.blogID == blogId)
                    .Include(x => x.SocialBlogImages)
                    .Include(x => x.SocialBlogContents)
                    .Include(x => x.SocialBlogTopics)
                    .Include(x => x.SocialBlogComments)
                    .Select(x => new
                    {
                        x.blogID,
                        x.blogTitle,
                        x.blogContent,
                        x.blogStatus,
                        x.blogLike,
                        x.blogImage,
                        x.blogDate,
                        x.updateAt,
                        x.blogDescription,
                        Images = x.SocialBlogImages.Select(i => new { i.srcImage, i.position }),
                        Contents = x.SocialBlogContents.Select(c => new { c.contentTitle, c.content, c.position }),
                        Topics = x.SocialBlogTopics.Select(t => new { t.topicTitle, t.topicContent, t.position }),
                        Comments = x.SocialBlogComments.Select(c => new { c.commentID, c.commentContent, c.commentDate })
                    })
                    .FirstOrDefaultAsync();

                if (blog == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy blog");
                }

                // Cache the blog data in Redis
                var response = ErrorConst.Success("Lấy blog thành công", blog);
                await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(response), TimeSpan.FromHours(1));

                return response;
            }
            catch (Exception ex)
            {
                return ErrorConst.Error(500, $"Lỗi khi lấy blog: {ex.Message}");
            }
        }
    }
}