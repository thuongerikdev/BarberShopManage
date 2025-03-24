using BM.Constant;
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

namespace BM.Social.ApplicationService.SocialModule.Implements.Src
{
    public class SocialSrcMessageService : SocialServiceBase, ISocialSrcMessageService
    {
        public SocialSrcMessageService(ILogger<SocialSrcMessageService> logger, SocialDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> SocialCreateSrcMessage(SocialCreateSrcMessageDto socialCreateSrcMessageDto)
        {
            _logger.LogInformation("SocialCreateSrcMessage called");
            try
            {
                var srcMessage = new SocialSrcMessage
                {
                    srcID = socialCreateSrcMessageDto.srcID,
                    messageID = socialCreateSrcMessageDto.messageID,
                    srcMessageDate = DateTime.Now
                };
                _dbContext.socialSrcMessages.Add(srcMessage);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo srcMessage thành công", srcMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateSrcMessage(SocialUpdateSrcMessageDto socialUpdateSrcMessageDto)
        {
            _logger.LogInformation("SocialUpdateSrcMessage called");
            try
            {
                var srcMessage = await _dbContext.socialSrcMessages.FindAsync(socialUpdateSrcMessageDto.srcMessageID);
                if (srcMessage == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy srcMessage");
                }
                srcMessage.srcID = socialUpdateSrcMessageDto.srcID;
                srcMessage.messageID = socialUpdateSrcMessageDto.messageID;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật srcMessage thành công", srcMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }



        }
        public async Task<ResponeDto> SocialDeleteSrcMessage(int srcMessageID)
        {
            _logger.LogInformation("SocialDeleteSrcMessage called");
            try
            {
                var srcMessage = await _dbContext.socialSrcMessages.FindAsync(srcMessageID);
                if (srcMessage == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy srcMessage");
                }
                _dbContext.socialSrcMessages.Remove(srcMessage);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa srcMessage thành công", srcMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetSrcMessage(int srcMessageID)
        {
            _logger.LogInformation("SocialGetSrcMessage called");
            try
            {
                var srcMessage = await _dbContext.socialSrcMessages.FindAsync(srcMessageID);
                if (srcMessage == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy srcMessage");
                }
                return ErrorConst.Success("Lấy srcMessage thành công", srcMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllSrcMessage()
        {
            _logger.LogInformation("SocialGetAllSrcMessage called");
            try
            {
                var srcMessages = await _dbContext.socialSrcMessages.ToListAsync();
                return ErrorConst.Success("Lấy danh sách srcMessage thành công", srcMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
