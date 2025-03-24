using BM.Constant;
using BM.Social.ApplicationService.Common;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Domain;
using BM.Social.Dtos;
using BM.Social.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.SocialModule.Implements.Src
{
    public class SocialSrcBlogService : SocialServiceBase , ISocialSrcBlogService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public SocialSrcBlogService (ILogger <SocialSrcBlogService> logger, SocialDbContext dbContext , ICloudinaryService cloudinaryService) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> SocialCreateSrcBlog(SocialCreateSrcBlogDto socialCreateSrcBlogDto)
        {
            _logger.LogInformation("SocialCreateSrcBlog called");
            try
            {
              
                var srcBlog = new SocialSrcBlog
                {
                    srcID = socialCreateSrcBlogDto.srcID,
                    blogID = socialCreateSrcBlogDto.blogID,
                    srcBlogDate = DateTime.Now,
                    position = socialCreateSrcBlogDto.position
                };
                _dbContext.socialSrcBlogs.Add(srcBlog);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo srcBlog thành công", srcBlog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateSrcBlog(SocialUpdateSrcBlogDto socialUpdateSrcBlogDto)
        {
            _logger.LogInformation("SocialUpdateSrcBlog called");
            try
            {
                var srcBlog = await _dbContext.socialSrcBlogs.FindAsync(socialUpdateSrcBlogDto.srcBlogID);
                if (srcBlog == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy srcBlog");
                }
                srcBlog.srcID = socialUpdateSrcBlogDto.srcID;
                srcBlog.blogID = socialUpdateSrcBlogDto.blogID;
                srcBlog.position = socialUpdateSrcBlogDto.position;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật srcBlog thành công", srcBlog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteSrcBlog(int srcBlogID)
        {
            _logger.LogInformation("SocialDeleteSrcBlog called");
            try
            {
                var srcBlog = await _dbContext.socialSrcBlogs.FindAsync(srcBlogID);
                if (srcBlog == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy srcBlog");
                }
                _dbContext.socialSrcBlogs.Remove(srcBlog);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa srcBlog thành công", srcBlog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetSrcBlog(int srcBlogID)
        {
            _logger.LogInformation("SocialGetSrcBlog called");
            try
            {
                var srcBlog = await _dbContext.socialSrcBlogs.FindAsync(srcBlogID);
                if (srcBlog == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy srcBlog");
                }
                return ErrorConst.Success("Lấy srcBlog thành công", srcBlog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllSrcBlog()
        {
            _logger.LogInformation("SocialGetAllSrcBlog called");
            try
            {
                var srcBlogs = await _dbContext.socialSrcBlogs.ToListAsync();
                return ErrorConst.Success("Lấy danh sách srcBlog thành công", srcBlogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }

}
