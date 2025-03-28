﻿using BM.Constant;
using BM.Social.ApplicationService.Common;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Domain;
using BM.Social.Dtos;
using BM.Social.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.SocialModule.Implements.Social
{
    public class SocialBlogService : SocialServiceBase, ISocialBlogService
    {
        public SocialBlogService(ILogger<SocialBlogService> logger, SocialDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> SocialCreateBlog(SocialCreateBlogDto socialCreateBlogDto)
        {
            _logger.LogInformation("SocialCreateBlog called");
            try
            {
                var blog = new SocialBlog
                {
                    blogContent = socialCreateBlogDto.blogContent,
                    blogTitle = socialCreateBlogDto.blogTitle,
                    blogStatus = socialCreateBlogDto.blogStatus,
                    blogLike = socialCreateBlogDto.blogLike,
                    blogDate = DateTime.Now
                };
                _dbContext.socialBlogs.Add(blog);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo blog thành công", blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateBlog(SocialUpdateBlogDto socialUpdateBlogDto)
        {
            _logger.LogInformation("SocialUpdateBlog called");
            try
            {
                var blog = await _dbContext.socialBlogs.FindAsync(socialUpdateBlogDto.blogID);
                if (blog == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog");
                }
                blog.blogContent = socialUpdateBlogDto.blogContent;
                blog.blogTitle = socialUpdateBlogDto.blogTitle;
                blog.blogStatus = socialUpdateBlogDto.blogStatus;
                blog.blogLike = socialUpdateBlogDto.blogLike;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật blog thành công", blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteBlog(int blogID)
        {
            _logger.LogInformation("SocialDeleteBlog called");
            try
            {
                var blog = await _dbContext.socialBlogs.FindAsync(blogID);
                if (blog == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog");
                }
                _dbContext.socialBlogs.Remove(blog);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa blog thành công", blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetBlog(int blogID)
        {
            _logger.LogInformation("SocialGetBlog called");
            try
            {
                var blog = await _dbContext.socialBlogs
                                .Include(b => b.SocialSrcBlogs)
                                .ThenInclude(sb => sb.SocialSrc)
                                .FirstOrDefaultAsync(b => b.blogID == blogID);
                if (blog == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog");
                }
                return ErrorConst.Success("Lấy blog thành công", blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllBlog()
        {
            _logger.LogInformation("SocialGetAllBlog called");
            try
            {
                var blogs = await _dbContext.socialBlogs.ToListAsync();
                return ErrorConst.Success("Lấy danh sách blog thành công", blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
