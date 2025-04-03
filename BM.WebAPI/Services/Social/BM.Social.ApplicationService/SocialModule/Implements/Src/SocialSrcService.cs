using BM.Constant;
using BM.Constant.Dto;
using BM.Shared.ApplicationService;
using BM.Social.ApplicationService.Common;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.Domain;
using BM.Social.Dtos;
using BM.Social.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.SocialModule.Implements.Src
{
    public class SocialSrcService : SocialServiceBase, ISocialSrcService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IDatabase _redisDb;
        private readonly AuthDataSend _authDataSend;
        public SocialSrcService(
            ILogger<SocialSrcService> logger,
            SocialDbContext dbContext,
            ICloudinaryService cloudinaryService,
            IConnectionMultiplexer redis,
            AuthDataSend authDataSend 

            ) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
            _redisDb = redis.GetDatabase();
            _authDataSend = authDataSend;
        }
        public async Task<ResponeDto> SocialCreateSrc(SocialCreateSrcDto socialCreateSrcDto)
        {
            _logger.LogInformation("SocialCreateSrc called");
            try
            {
                if (socialCreateSrcDto == null)
                {
                    _logger.LogError("socialCreateSrcDto is null");
                    return ErrorConst.Error(400, "Dữ liệu đầu vào không hợp lệ");
                }

                string imageSrc = null;
                if (socialCreateSrcDto.imageSrc != null)
                {
                    if (socialCreateSrcDto.imageSrc.Length == 0)
                    {
                        _logger.LogWarning("imageSrc is empty");
                        return ErrorConst.Error(400, "File ảnh trống");
                    }

                    _logger.LogInformation("Uploading image to Cloudinary");
                    imageSrc = await _cloudinaryService.UploadImageAsync(socialCreateSrcDto.imageSrc);
                    if (imageSrc == null)
                    {
                        _logger.LogWarning("Failed to upload image to Cloudinary");
                        return ErrorConst.Error(500, "Không thể tải ảnh lên Cloudinary");
                    }
                }
                else
                {
                    _logger.LogInformation("No image provided in request");
                }

                var src = new SocialSrc
                {
                    srcDate = DateTime.Now,
                    srcName = socialCreateSrcDto.srcName,
                    srcTitle = socialCreateSrcDto.srcTitle,
                    imageSrc = imageSrc
                };

                _logger.LogInformation("Adding src to database");
                _dbContext.socialSrcs.Add(src);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Src created successfully with ID: {SrcId}", src.srcID);
                return ErrorConst.Success("Tạo src thành công", src);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SocialCreateSrc");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateSrc(SocialUpdateSrcDto socialUpdateSrcDto)
        {
            _logger.LogInformation("SocialUpdateSrc called");
            try
            {
                var src = await _dbContext.socialSrcs.FindAsync(socialUpdateSrcDto.srcID);
                if (src == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy src");
                }
                src.srcDate = DateTime.Now;
                src.srcName = socialUpdateSrcDto.srcName;
                src.imageSrc = socialUpdateSrcDto.imageSrc;
                src.srcTitle = socialUpdateSrcDto.srcTitle;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật src thành công", src);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteSrc(int srcID)
        {
            _logger.LogInformation("SocialDeleteSrc called");
            try
            {
                var src = await _dbContext.socialSrcs.FindAsync(srcID);
                if (src == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy src");
                }
                _dbContext.socialSrcs.Remove(src);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa src thành công", src);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetSrc(int srcID)
        {
            _logger.LogInformation("SocialGetSrc called");
            try
            {
                var src = await _dbContext.socialSrcs.FindAsync(srcID);
                if (src == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy src");
                }
                return ErrorConst.Success("Lấy src thành công", src);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllSrc()
        {
            _logger.LogInformation("SocialGetAllSrc called");
            try
            {
                var srcs = await _dbContext.socialSrcs.ToListAsync();
                return ErrorConst.Success("Lấy danh sách src thành công", srcs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }

        }
        public async Task<ResponeDto> GetSocialSrcbyType(string srcType)
        {
            _logger.LogInformation("SocialGetSliderImage called with srcType: {SrcType}", srcType);
            try
            {
                string redisKey = $"social_src:{srcType}";
                var cachedData = await _redisDb.StringGetAsync(redisKey);

                if (cachedData.HasValue)
                {
                    _logger.LogInformation("Data retrieved from Redis with key: {RedisKey}", redisKey);
                    var srcListFromCache = JsonSerializer.Deserialize<List<SocialSrc>>(cachedData);
                    return ErrorConst.Success("Lấy danh sách từ cache thành công", srcListFromCache);
                }

                // Nếu không có trong Redis, lấy từ database
                var srcList = await _dbContext.socialSrcs
                    .Where(x => x.srcTitle == srcType)
                    .ToListAsync();

                if (srcList == null || !srcList.Any())
                {
                    _logger.LogWarning("No items found for srcType: {SrcType}", srcType);
                    return ErrorConst.Error(500, "Lấy danh sách thất bại");
                }

                // Chuyển thành JSON và lưu vào Redis
                string jsonData = JsonSerializer.Serialize(srcList);
                await _redisDb.StringSetAsync(redisKey, jsonData, TimeSpan.FromHours(1));
                _logger.LogInformation("Data saved to Redis with key: {RedisKey}", redisKey);

                return ErrorConst.Success("Lấy danh sách thành công", srcList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSocialSrcbyType");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        //public async Task<ResponeDto> SocialUpdateAvatar (int userID , IFormFile file )
        //{
        //    _logger.LogInformation("Update user avataer");
        //    try
        //    {
        //        var userData = await _authDataSend.GetUserToOtherDomain(userID);
        //        var user = userData.Data as AuthUser; 
        //        user.

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex , ex.Message);
        //        return ErrorConst.Error(500 ,ex.Message);
        //    }
        //}
    }

}
