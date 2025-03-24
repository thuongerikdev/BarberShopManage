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
    public class SocialGroupService : SocialServiceBase, ISocialGroupService
    {
        public SocialGroupService(ILogger<SocialGroupService> logger, SocialDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> SocialCreateGroup(SocialCreateGroupDto socialCreateGroupDto)
        {
            _logger.LogInformation("SocialCreateGroup called");
            try
            {
                var group = new SocialGroup
                {
                    groupName = socialCreateGroupDto.groupName,
                    groupDescription = socialCreateGroupDto.groupDescription,
                    groupDate = DateTime.Now,
                    groupStatus = socialCreateGroupDto.groupStatus,
                    srcID = 0
                };
                _dbContext.socialGroups.Add(group);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo group thành công", group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateGroup(SocialUpdateGroupDto socialUpdateGroupDto)
        {
            _logger.LogInformation("SocialUpdateGroup called");
            try
            {
                var group = await _dbContext.socialGroups.FindAsync(socialUpdateGroupDto.groupID);
                if (group == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy group");
                }
                group.groupName = socialUpdateGroupDto.groupName;
                group.groupDescription = socialUpdateGroupDto.groupDescription;
                group.groupStatus = socialUpdateGroupDto.groupStatus;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật group thành công", group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteGroup(int groupID)
        {
            _logger.LogInformation("SocialDeleteGroup called");
            try
            {
                var group = await _dbContext.socialGroups.FindAsync(groupID);
                if (group == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy group");
                }
                _dbContext.socialGroups.Remove(group);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa group thành công", group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetGroup(int groupID)
        {
            _logger.LogInformation("SocialGetGroup called");
            try
            {
                var group = await _dbContext.socialGroups.FindAsync(groupID);
                if (group == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy group");
                }
                return ErrorConst.Success("Lấy group thành công", group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllGroup()
        {
            _logger.LogInformation("SocialGetAllGroup called");
            try
            {
                var groups = await _dbContext.socialGroups.ToListAsync();
                return ErrorConst.Success("Lấy danh sách group thành công", groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }

        }
    }
}
