using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.Dtos
{
    public class SocialCreateBlogDto
    {
        public string blogTitle { get; set; }
        public string blogContent { get; set; }
        public string blogStatus { get; set; }
        public int blogLike { get; set; } = 0;
        public string blogDescription { get; set; }
        public IFormFile blogImage { get; set; }
    }

    public class SocialCreateBlogTopicDto
    {
   
        public int blogID { get; set; }
        public int position { get; set; }
        public string topicTitle { get; set; }
        public string topicContent { get; set; }
        public string topicStatus { get; set; }
        public DateTime topicDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
    }
    public class SocialUpdateBlogTopicDto
    {
        public int topicID { get; set; }
        public int blogID { get; set; }
        public int position { get; set; }
        public string topicTitle { get; set; }
        public string topicContent { get; set; }
        public string topicStatus { get; set; }
     
    }
    public class SocialCreateBlogImageDto
    {
        public int blogID { get; set; }
        public int position { get; set; }
        public IFormFile srcImage { get; set; }
        public string status { get; set; }
    }
    

    public class SocialUpdateBlogImageDto
    {
        public int blogImageID { get; set; }
        public int blogID { get; set; }
        public int position { get; set; }
        public IFormFile srcImage { get; set; }
        public string status { get; set; }
    }
    public class SocialCreateBlogContentDto
    {
        public int blogID { get; set; }
        public int position { get; set; }
        public string contentTitle { get; set; }
        public string status { get; set; }
        public string content { get; set; }
        public DateTime contentDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
    }

    public class SocicalCreateBlogBussiness
    {
        public int blogID { get; set; }
        public List<SocialCreateBlogTopicBussiness> topics { get; set; }  = new List<SocialCreateBlogTopicBussiness>();
        public  List<SocialCreateBlogContentBussiness> contents { get; set; } = new List<SocialCreateBlogContentBussiness>();
        public List<SocialCreateBlogImageBussiness> images { get; set; } = new List<SocialCreateBlogImageBussiness>();

    }
    public class SocialCreateBlogContentBussiness
    {
        public int position { get; set; }
        public string contentTitle { get; set; }
        public string content { get; set; }
    }
    public class SocialCreateBlogImageBussiness
    {
        public int position { get; set; }
        public IFormFile srcImage { get; set; }
    }
    public class SocialCreateBlogTopicBussiness
    {
        public int position { get; set; }
        public string topicTitle { get; set; }
        public string topicContent { get; set; }
    }


    public class SocialUpdateBlogContentDto
    {
        public int contentID { get; set; }
        public int blogID { get; set; }
        public int position { get; set; }
        public string content { get; set; }
        public string contentTitle { get; set; }
      
        public string status { get; set; }
    }



    public class SocialUpdateBlogDto
    {
        public int blogID { get; set; }
        public string blogTitle { get; set; }
        public string blogContent { get; set; }
        public string blogStatus { get; set; }
        public int blogLike { get; set; }
        public string blogDescription { get; set; }
        public IFormFile blogImage { get; set; }
    }
    public class SocialCreateCommentDto
    {
        public int blogID { get; set; }
        public int userID { get; set; }
        public string commentContent { get; set; }
        public string commentStatus { get; set; }
        public int commentLike { get; set; }


    }
    public class SocialUpdateCommentDto
    {
        public int commentID { get; set; }
        public int blogID { get; set; }
        public int userID { get; set; }
        public string commentContent { get; set; }
        public string commentStatus { get; set; }
        public int commentLike { get; set; }
    }
   
    //public class CreateBlogRequestDto
    //{
    //    [Required(ErrorMessage = "Tiêu đề blog là bắt buộc")]
    //    public string BlogTitle { get; set; }

    //    [Required(ErrorMessage = "Trạng thái blog là bắt buộc")]
    //    public string BlogStatus { get; set; }

    //    [Required(ErrorMessage = "Danh sách topic là bắt buộc")]
    //    [MinLength(1, ErrorMessage = "Phải có ít nhất một topic")]
    //    public List<string> Topics { get; set; }

    //    [Required(ErrorMessage = "Danh sách nội dung là bắt buộc")]
    //    [MinLength(1, ErrorMessage = "Phải có ít nhất một nội dung")]
    //    public List<string> Contents { get; set; }

    //    public List<IFormFile>? Images { get; set; } // Có thể null
    //}


    //public class BlogImageDto
    //{
    //    public int SrcId { get; set; }
    //    public string SrcName { get; set; }
    //    public string SrcUrl { get; set; }
    //    public int Position { get; set; }
    //}







}
