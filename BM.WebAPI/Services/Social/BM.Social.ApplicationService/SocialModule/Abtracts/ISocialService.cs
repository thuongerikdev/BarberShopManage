using BM.Constant;
using BM.Social.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.SocialModule.Abtracts
{
    public interface ISocialBlogService
    {
        public  Task<ResponeDto> SocialCreateBlog(SocialCreateBlogDto socialCreateBlogDto);
        public Task<ResponeDto> SocialUpdateBlog(SocialUpdateBlogDto socialUpdateBlogDto);
        public Task<ResponeDto> SocialDeleteBlog(int blogID);
        public Task<ResponeDto> SocialGetBlog(int blogID);
        public Task<ResponeDto> SocialGetAllBlog();
    }
    public interface ISocialBlogTopicService
    {
        public Task<ResponeDto> SocialCreateBlogTopic(SocialCreateBlogTopicDto socialCreateBlogTopicDto);
        public Task<ResponeDto> SocialUpdateBlogTopic(SocialUpdateBlogTopicDto socialUpdateBlogTopicDto);
        public Task<ResponeDto> SocialDeleteBlogTopic(int blogTopicID);
        public Task<ResponeDto> SocialGetBlogTopic(int blogTopicID);
        public Task<ResponeDto> SocialGetAllBlogTopic();
    }
    public interface ISocialBlogImageService
    {
        public Task<ResponeDto> SocialCreateBlogImage(SocialCreateBlogImageDto socialCreateBlogImageDto);
        public Task<ResponeDto> SocialUpdateBlogImage(SocialUpdateBlogImageDto socialUpdateBlogImageDto);
        public Task<ResponeDto> SocialDeleteBlogImage(int blogImageID);
        public Task<ResponeDto> SocialGetBlogImage(int blogImageID);
        public Task<ResponeDto> SocialGetAllBlogImage();
    }
    public interface ISocialBlogContentService
    {
        public Task<ResponeDto> SocialCreateBlogContent(SocialCreateBlogContentDto socialCreateBlogContentDto);
        public Task<ResponeDto> SocialUpdateBlogContent(SocialUpdateBlogContentDto socialUpdateBlogContentDto);
        public Task<ResponeDto> SocialDeleteBlogContent(int blogContentID);
        public Task<ResponeDto> SocialGetBlogContent(int blogContentID);
        public Task<ResponeDto> SocialGetAllBlogContent();
    }
    public interface ISocialCommentService
    {
        public Task<ResponeDto> SocialCreateComment(SocialCreateCommentDto socialCreateCommentDto);
        public Task<ResponeDto> SocialUpdateComment(SocialUpdateCommentDto socialUpdateCommentDto);
        public Task<ResponeDto> SocialDeleteComment(int commentID);
        public Task<ResponeDto> SocialGetComment(int commentID);
        public Task<ResponeDto> SocialGetAllComment();
    }
    public interface ISocialMessageService
    {
        public Task<ResponeDto> SocialCreateMessage(SocialCreateMessageDto socialCreateMessageDto);
        public Task<ResponeDto> SocialUpdateMessage(SocialUpdateMessageDto socialUpdateMessageDto);
        public Task<ResponeDto> SocialDeleteMessage(int messageID);
        public Task<ResponeDto> SocialGetMessage(int messageID);
        public Task<ResponeDto> SocialGetAllMessage();
    }
    public interface ISocialGroupService
    {
        public Task<ResponeDto> SocialCreateGroup(SocialCreateGroupDto socialCreateGroupDto);
        public Task<ResponeDto> SocialUpdateGroup(SocialUpdateGroupDto socialUpdateGroupDto);
        public Task<ResponeDto> SocialDeleteGroup(int groupID);
        public Task<ResponeDto> SocialGetGroup(int groupID);
        public Task<ResponeDto> SocialGetAllGroup();
    }
    public interface  ISocialGroupUserService
    {
        public Task<ResponeDto> SocialCreateGroupUser(SocialCreateGroupUserDto socialCreateGroupUserDto);
        public Task<ResponeDto> SocialUpdateGroupUser(SocialUpdateGroupUserDto socialUpdateGroupUserDto);
        public Task<ResponeDto> SocialDeleteGroupUser(int groupUserID);
        public Task<ResponeDto> SocialGetGroupUser(int groupUserID);
        public Task<ResponeDto> SocialGetAllGroupUser();

    }
}
