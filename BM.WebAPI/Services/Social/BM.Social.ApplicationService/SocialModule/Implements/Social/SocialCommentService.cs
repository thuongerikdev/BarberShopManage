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
    public class SocialCommentService : SocialServiceBase, ISocialCommentService
    {
        public SocialCommentService(ILogger<SocialCommentService> logger, SocialDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> SocialCreateComment(SocialCreateCommentDto socialCreateCommentDto)
        {
            _logger.LogInformation("SocialCreateComment");
            try
            {
                var comment = new SocialBlogComment
                {
                    commentContent = socialCreateCommentDto.commentContent,
                    commentDate = DateTime.Now,
                    commentStatus = socialCreateCommentDto.commentStatus,
                    commentLike = 0,
                    blogID = socialCreateCommentDto.blogID,
                    userID = socialCreateCommentDto.userID

                };
                _dbContext.socialBlogComments.Add(comment);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo comment thành công", comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialUpdateComment(SocialUpdateCommentDto socialUpdateCommentDto)
        {
            _logger.LogInformation("SocialUpdateComment");
            try
            {
                var comment = await _dbContext.socialBlogComments.FindAsync(socialUpdateCommentDto.commentID);
                if (comment == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy comment");
                }
                comment.commentContent = socialUpdateCommentDto.commentContent;
                comment.commentStatus = socialUpdateCommentDto.commentStatus;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật comment thành công", comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialDeleteComment(int commentID)
        {
            _logger.LogInformation("SocialDeleteComment");
            try
            {
                var comment = await _dbContext.socialBlogComments.FindAsync(commentID);
                if (comment == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy comment");
                }
                _dbContext.socialBlogComments.Remove(comment);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa comment thành công", comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetComment(int commentID)
        {
            _logger.LogInformation("SocialGetComment");
            try
            {
                var comment = await _dbContext.socialBlogComments.FindAsync(commentID);
                if (comment == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy comment");
                }
                return ErrorConst.Success("Lấy comment thành công", comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> SocialGetAllComment()
        {
            _logger.LogInformation("SocialGetAllComment");
            try
            {
                var comments = await _dbContext.socialBlogComments.ToListAsync();
                return ErrorConst.Success("Lấy danh sách comment thành công", comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
