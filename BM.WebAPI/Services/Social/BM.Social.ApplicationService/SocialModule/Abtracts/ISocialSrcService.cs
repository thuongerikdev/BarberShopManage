using BM.Constant;
using BM.Social.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.SocialModule.Abtracts
{
    public interface ISocialSrcService
    {
        public Task<ResponeDto> SocialCreateSrc(SocialCreateSrcDto socialCreateSrcDto);
        public Task<ResponeDto> SocialUpdateSrc(SocialUpdateSrcDto socialUpdateSrcDto);
        public Task<ResponeDto> SocialDeleteSrc(int srcID);
        public Task<ResponeDto> SocialGetSrc(int srcID);
        public Task<ResponeDto> SocialGetAllSrc();
        public Task<ResponeDto> GetSocialSrcbyType(string srcType);
    }
    public interface ISocialSrcBlogService
    {
        public Task<ResponeDto> SocialCreateSrcBlog(SocialCreateSrcBlogDto socialCreateSrcBlogDto);
        public Task<ResponeDto> SocialUpdateSrcBlog(SocialUpdateSrcBlogDto socialUpdateSrcBlogDto);
        public Task<ResponeDto> SocialDeleteSrcBlog(int srcBlogID);
        public Task<ResponeDto> SocialGetSrcBlog(int srcBlogID);
        public Task<ResponeDto> SocialGetAllSrcBlog();
    }
    public interface ISocialSrcMessageService
    {
        public Task<ResponeDto> SocialCreateSrcMessage(SocialCreateSrcMessageDto socialCreateSrcMessageDto);
        public Task<ResponeDto> SocialUpdateSrcMessage(SocialUpdateSrcMessageDto socialUpdateSrcMessageDto);
        public Task<ResponeDto> SocialDeleteSrcMessage(int srcMessageID);
        public Task<ResponeDto> SocialGetSrcMessage(int srcMessageID);
        public Task<ResponeDto> SocialGetAllSrcMessage();
    }
}
