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
using Microsoft.Extensions.Logging;

namespace BM.Social.ApplicationService.SocialModule.Implements.Social
{
    public class SocialBlogContentService : SocialServiceBase, ISocialBlogContentService
    {
        public SocialBlogContentService(ILogger<SocialBlogContentService> logger, SocialDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> SocialCreateBlogContent(SocialCreateBlogContentDto socialCreateBlogContentDto)
        {
            _logger.LogInformation("SocialCreateBlogContent called");
            try
            {
                var blogContent = new SocialBlogContent
                {
                    blogID = socialCreateBlogContentDto.blogID,
                    contentTitle = socialCreateBlogContentDto.contentTitle,
                    position = socialCreateBlogContentDto.position,
                    content = socialCreateBlogContentDto.content,
                    contentDate = DateTime.Now,
                    status = socialCreateBlogContentDto.status,
                    updateAt = DateTime.Now
                };
                _dbContext.socialBlogContents.Add(blogContent);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo blog content thành công", blogContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateBlogContent(SocialUpdateBlogContentDto socialUpdateBlogContentDto)
        {
            _logger.LogInformation("SocialUpdateBlogContent called");
            try
            {
                var blogContent = await _dbContext.socialBlogContents.FindAsync(socialUpdateBlogContentDto.contentID);
                if (blogContent == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog content");
                }
                blogContent.blogID = socialUpdateBlogContentDto.blogID;
                blogContent.contentTitle = socialUpdateBlogContentDto.contentTitle;
                blogContent.position = socialUpdateBlogContentDto.position;
                blogContent.content = socialUpdateBlogContentDto.content;
                blogContent.status = socialUpdateBlogContentDto.status;
                blogContent.updateAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật blog content thành công", blogContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteBlogContent(int contentID)
        {
            _logger.LogInformation("SocialDeleteBlogContent called");
            try
            {
                var blogContent = await _dbContext.socialBlogContents.FindAsync(contentID);
                if (blogContent == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog content");
                }
                _dbContext.socialBlogContents.Remove(blogContent);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa blog content thành công", blogContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetBlogContent(int blogID)
        {
            _logger.LogInformation("SocialGetBlogContent called");
            try
            {
                var blogContent = await _dbContext.socialBlogContents.Where(x => x.blogID == blogID).ToListAsync();
                if (blogContent == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog content");
                }
                return ErrorConst.Success("Lấy blog content thành công", blogContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetBlogContentByID(int contentID)
        {
            _logger.LogInformation("SocialGetBlogContentByID called");
            try
            {
                var blogContent = await _dbContext.socialBlogContents.FindAsync(contentID);
                if (blogContent == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog content");
                }
                return ErrorConst.Success("Lấy blog content thành công", blogContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task <ResponeDto> SocialGetAllBlogContent()
        {
            _logger.LogInformation("SocialGetAllBlogContent called");
            try
            {
                var blogContent = await _dbContext.socialBlogContents.ToListAsync();
                if (blogContent == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog content");
                }
                return ErrorConst.Success("Lấy tất cả blog content thành công", blogContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
