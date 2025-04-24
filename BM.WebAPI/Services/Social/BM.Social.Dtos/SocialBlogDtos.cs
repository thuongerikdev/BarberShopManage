using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
    public class SocialCreateMessageDto
    {
        public int senderID { get; set; }
        public int receiverID { get; set; }
        public int groupID { get; set; }
        public string messageContent { get; set; }
        public string messageStatus { get; set; }
    }
    public class SocialUpdateMessageDto
    {
        public int messageID { get; set; }
        public int senderID { get; set; }
        public int receiverID { get; set; }
        public int groupID { get; set; }
        public string messageContent { get; set; }
        public string messageStatus { get; set; }
    }
    public class SocialCreateGroupDto
    {
        public string groupName { get; set; }
        public string groupDescription { get; set; }
        public string groupStatus { get; set; }
        //public int srcID { get; set; }
    }
    public class SocialUpdateGroupDto
    {
        public int groupID { get; set; }
        public string groupName { get; set; }
        public string groupDescription { get; set; }
        public string groupStatus { get; set; }
        public int srcID { get; set; }
    }
    public class SocialCreateGroupUserDto
    {
        public int groupID { get; set; }
        public int userID { get; set; }
    }
    public class SocialUpdateGroupUserDto
    {
        public int groupUserID { get; set; }
        public int groupID { get; set; }
        public int userID { get; set; }
    }

    public class CreateBlogRequestDto
    {
        public string BlogTitle { get; set; }
        public string BlogStatus { get; set; }
        public List<string> Topics { get; set; }
        public List<string> Contents { get; set; }
        public  List<IFormFile> Images { get; set; }
    }



}
