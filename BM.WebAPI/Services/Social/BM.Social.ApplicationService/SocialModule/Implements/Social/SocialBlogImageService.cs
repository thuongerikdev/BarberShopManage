using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM.Constant;
using BM.Shared.ApplicationService;
using BM.Social.ApplicationService.Common;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Domain;
using BM.Social.Dtos;
using BM.Social.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BM.Social.ApplicationService.SocialModule.Implements.Social
{
    public class SocialBlogImageService : SocialServiceBase, ISocialBlogImageService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public SocialBlogImageService(ILogger<SocialBlogImageService> logger, SocialDbContext dbContext , ICloudinaryService cloudinaryService) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> SocialCreateBlogImage(SocialCreateBlogImageDto socialCreateBlogImageDto)
        {
            _logger.LogInformation("SocialCreateBlogImage called");
            try
            {
                var img = await _cloudinaryService.UploadImageAsync(socialCreateBlogImageDto.srcImage);
                if (img == null)
                {
                    _logger.LogError("img is null");
                    return ErrorConst.Error(400, "Dữ liệu đầu vào không hợp lệ");
                }
                var blogImage = new SocialBlogImage
                {
                    blogID = socialCreateBlogImageDto.blogID,
                    srcImage = img,
                    topicImageDate = DateTime.Now,
                    position = socialCreateBlogImageDto.position,
                    status = socialCreateBlogImageDto.status,
                    updateAt = DateTime.Now

                };
                _dbContext.socialBlogImages.Add(blogImage);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo blog image thành công", blogImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateBlogImage(SocialUpdateBlogImageDto socialUpdateBlogImageDto)
        {
            _logger.LogInformation("SocialUpdateBlogImage called");
            try
            {
                var img = await _cloudinaryService.UploadImageAsync(socialUpdateBlogImageDto.srcImage);
                var blogImage = await _dbContext.socialBlogImages.FindAsync(socialUpdateBlogImageDto.blogImageID);
                if (blogImage == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog image");
                }
                blogImage.blogID = socialUpdateBlogImageDto.blogID;
                blogImage.srcImage = img;
                blogImage.position = socialUpdateBlogImageDto.position;
                blogImage.status = socialUpdateBlogImageDto.status;
                blogImage.updateAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật blog image thành công", blogImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteBlogImage(int blogImageID)
        {
            _logger.LogInformation("SocialDeleteBlogImage called");
            try
            {
                var blogImage = await _dbContext.socialBlogImages.FindAsync(blogImageID);
                if (blogImage == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog image");
                }
                _dbContext.socialBlogImages.Remove(blogImage);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa blog image thành công", blogImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetBlogImage(int blogImageID)
        {
            _logger.LogInformation("SocialGetBlogImage called");
            try
            {
                var blogImage = await _dbContext.socialBlogImages.FindAsync(blogImageID);
                if (blogImage == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog image");
                }
                return ErrorConst.Success("Lấy blog image thành công", blogImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllBlogImage()
        {
            _logger.LogInformation("SocialGetAllBlogImage called");
            try
            {
                var blogImages = await _dbContext.socialBlogImages.ToListAsync();
                if (blogImages == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy blog image");
                }
                return ErrorConst.Success("Lấy tất cả blog image thành công", blogImages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
