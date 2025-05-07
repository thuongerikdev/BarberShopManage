using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM.Constant;
using BM.Social.ApplicationService.Common;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Domain;
using BM.Social.Dtos;
using BM.Social.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BM.Social.ApplicationService.SocialModule.Implements.Social
{
    public class SocialBlogTopicService : SocialServiceBase, ISocialBlogTopicService
    {
        public SocialBlogTopicService(ILogger<SocialBlogTopicService> logger, SocialDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task<ResponeDto> SocialCreateBlogTopic(SocialCreateBlogTopicDto socialCreateBlogTopicDto)
        {
            _logger.LogInformation("SocialCreateBlogTopic called");
            try
            {
                var blogTopic = new SocailBlogTopic
                {
                    blogID = socialCreateBlogTopicDto.blogID,
                    topicContent = socialCreateBlogTopicDto.topicContent,
                    topicTitle = socialCreateBlogTopicDto.topicTitle,
                    topicStatus = socialCreateBlogTopicDto.topicStatus,
                    topicDate = DateTime.Now,
                    updateAt = DateTime.Now,
                    position = socialCreateBlogTopicDto.position

                };
                _dbContext.socialBlogTopics.Add(blogTopic);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo blog topic thành công", blogTopic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateBlogTopic(SocialUpdateBlogTopicDto socialUpdateBlogTopicDto)
        {
            _logger.LogInformation("SocialUpdateBlogTopic called");
            try
            {
                var blogTopic = await _dbContext.socialBlogTopics.FindAsync(socialUpdateBlogTopicDto.topicID);
                if (blogTopic == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog topic");
                }
                blogTopic.blogID = socialUpdateBlogTopicDto.blogID;
                blogTopic.topicContent = socialUpdateBlogTopicDto.topicContent;
                blogTopic.topicTitle = socialUpdateBlogTopicDto.topicTitle;
                blogTopic.topicStatus = socialUpdateBlogTopicDto.topicStatus;
                blogTopic.updateAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật blog topic thành công", blogTopic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteBlogTopic(int topicID)
        {
            _logger.LogInformation("SocialDeleteBlogTopic called");
            try
            {
                var blogTopic = await _dbContext.socialBlogTopics.FindAsync(topicID);
                if (blogTopic == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog topic");
                }
                _dbContext.socialBlogTopics.Remove(blogTopic);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa blog topic thành công", blogTopic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetBlogTopic(int topicID)
        {
            _logger.LogInformation("SocialGetBlogTopic called");
            try
            {
                var blogTopic = await _dbContext.socialBlogTopics.FindAsync(topicID);
                if (blogTopic == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog topic");
                }
                return ErrorConst.Success("Lấy blog topic thành công", blogTopic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllBlogTopic()
        {
            _logger.LogInformation("SocialGetAllBlogTopic called");
            try
            {
                var blogTopics = await _dbContext.socialBlogTopics.ToListAsync();
                if (blogTopics == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog topic");
                }
                return ErrorConst.Success("Lấy tất cả blog topic thành công", blogTopics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }



    }
   
}
