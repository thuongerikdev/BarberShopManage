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

namespace BM.Social.ApplicationService.SocialModule.Implements.Social
{
    public class SocialMessageService : SocialServiceBase, ISocialMessageService
    {
        public SocialMessageService(ILogger<SocialMessageService> logger, SocialDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> SocialCreateMessage(SocialCreateMessageDto socialCreateMessageDto)
        {
            _logger.LogInformation("SocialCreateMessage");
            try
            {
                var message = new SocialMessage
                {
                    messageContent = socialCreateMessageDto.messageContent,
                    messageDate = DateTime.Now,
                    messageStatus = socialCreateMessageDto.messageStatus,
                    senderID = socialCreateMessageDto.senderID,
                    receiverID = socialCreateMessageDto.receiverID
                };
                _dbContext.socialMessages.Add(message);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo message thành công", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateMessage(SocialUpdateMessageDto socialUpdateMessageDto)
        {
            _logger.LogInformation("SocialUpdateMessage");
            try
            {
                var message = await _dbContext.socialMessages.FindAsync(socialUpdateMessageDto.messageID);
                if (message == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy message");
                }
                message.messageContent = socialUpdateMessageDto.messageContent;
                message.messageDate = DateTime.Now;
                message.messageStatus = socialUpdateMessageDto.messageStatus;
                message.senderID = socialUpdateMessageDto.senderID;
                message.receiverID = socialUpdateMessageDto.receiverID;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật message thành công", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteMessage(int messageID)
        {
            _logger.LogInformation("SocialDeleteMessage");
            try
            {
                var message = await _dbContext.socialMessages.FindAsync(messageID);
                if (message == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy message");
                }
                _dbContext.socialMessages.Remove(message);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa message thành công", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetMessage(int messageID)
        {
            _logger.LogInformation("SocialGetMessage");
            try
            {
                var message = await _dbContext.socialMessages.FindAsync(messageID);
                if (message == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy message");
                }
                return ErrorConst.Success("Lấy message thành công", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllMessage()
        {
            _logger.LogInformation("SocialGetAllMessage");
            try
            {
                var messages = await _dbContext.socialMessages.ToListAsync();
                return ErrorConst.Success("Lấy danh sách message thành công", messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
