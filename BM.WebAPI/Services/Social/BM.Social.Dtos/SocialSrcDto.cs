using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.Dtos
{
    public class SocialCreateSrcDto
    {
        public string srcName { get; set; }
        public IFormFile imageSrc { get; set; }


    }
    public class SocialUpdateSrcDto
    {
        public int srcID { get; set; }
        public string srcName { get; set; }

        public string imageSrc { get; set; }
        public string videoSrc { get; set; }
    }
    public class SocialCreateSrcBlogDto
    {
        public int srcID { get; set; }
        public int blogID { get; set; }
        public int position { get; set; }
    }
    public class SocialUpdateSrcBlogDto
    {
        public int srcBlogID { get; set; }
        public int srcID { get; set; }
        public int blogID { get; set; }
        public int position { get; set; }
    }
    public class SocialCreateSrcMessageDto
    {
        public int srcID { get; set; }
        public int messageID { get; set; }
    }
    public class SocialUpdateSrcMessageDto
    {
        public int srcMessageID { get; set; }
        public int srcID { get; set; }
        public int messageID { get; set; }
    }

    public class SocialBlogCacheDto
    {
        public int BlogID { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public string BlogStatus { get; set; }
        public int BlogLike { get; set; }
        public DateTime CreatedDate { get; set; }
        // Không bao gồm SocialSrcBlogs để tránh vòng lặp
    }

    public class ResponeCacheDto
    {
        public int ErrorCode { get; set; }
        public string ErrorMessager { get; set; }
        public SocialBlogCacheDto Data { get; set; }
    }
}
