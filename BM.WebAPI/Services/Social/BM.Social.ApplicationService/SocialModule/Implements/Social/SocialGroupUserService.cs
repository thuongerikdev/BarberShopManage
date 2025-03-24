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
    public class SocialGroupUserService : SocialServiceBase, ISocialGroupUserService
    {
        public SocialGroupUserService(ILogger<SocialGroupUserService> logger, SocialDbContext dbContext) : base(logger, dbContext)
        {

        }
        public async Task<ResponeDto> SocialCreateGroupUser(SocialCreateGroupUserDto socialCreateGroupUserDto)
        {
            _logger.LogInformation("SocialCreateGroupUser called");
            try
            {
                var groupUser = new SocialGroupUser
                {
                    groupID = socialCreateGroupUserDto.groupID,
                    userID = socialCreateGroupUserDto.userID,

                };
                _dbContext.socialGroupUsers.Add(groupUser);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo group user thành công", groupUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateGroupUser(SocialUpdateGroupUserDto socialUpdateGroupUserDto)
        {
            _logger.LogInformation("SocialUpdateGroupUser called");
            try
            {
                var groupUser = await _dbContext.socialGroupUsers.FindAsync(socialUpdateGroupUserDto.groupUserID);
                if (groupUser == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy group user");
                }
                groupUser.groupID = socialUpdateGroupUserDto.groupID;
                groupUser.userID = socialUpdateGroupUserDto.userID;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật group user thành công", groupUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteGroupUser(int groupUserID)
        {
            _logger.LogInformation("SocialDeleteGroupUser called");
            try
            {
                var groupUser = await _dbContext.socialGroupUsers.FindAsync(groupUserID);
                if (groupUser == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy group user");
                }
                _dbContext.socialGroupUsers.Remove(groupUser);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa group user thành công", groupUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetGroupUser(int groupUserID)
        {
            _logger.LogInformation("SocialGetGroupUser called");
            try
            {
                var groupUser = await _dbContext.socialGroupUsers.FindAsync(groupUserID);
                if (groupUser == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy group user");
                }
                return ErrorConst.Success("Lấy group user thành công", groupUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllGroupUser()
        {
            _logger.LogInformation("SocialGetAllGroupUser called");
            try
            {
                var groupUsers = await _dbContext.socialGroupUsers.ToListAsync();
                return ErrorConst.Success("Lấy danh sách group user thành công", groupUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
