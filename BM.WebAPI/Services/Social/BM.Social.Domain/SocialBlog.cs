using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.Domain
{
    [Table(nameof(SocialBlog), Schema = BM.Constant.Database.DbSchema.Social)]
    public class SocialBlog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int blogID { get; set; }
        public string blogTitle { get; set; }
        public string blogContent { get; set; }
        public string blogStatus { get; set; }
        public string blogDescription { get; set; }
        public string blogImage { get; set; }
        public int blogLike { get; set; }
        public DateTime blogDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual ICollection<SocialBlogComment> SocialBlogComments { get; set; }
        public virtual ICollection<SocailBlogTopic> SocialBlogTopics { get; set; }
        public virtual ICollection<SocialBlogImage> SocialBlogImages { get; set; }
        public virtual ICollection<SocialBlogContent> SocialBlogContents { get; set; }

    }


    public class SocailBlogTopic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int topicID { get; set; }
        public int blogID { get; set; }
        public int position { get; set; }
        public string topicTitle { get; set; }
        public string topicContent { get; set; }
        public string topicStatus { get; set; }
        public DateTime topicDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual SocialBlog SocialBlog { get; set; }
       

    }
   

    public class SocialBlogImage 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int blogImageID { get; set; }
        public int blogID { get; set; }
        public int position { get; set; }
        public string srcImage { get; set; }
        public string status { get; set; }
        public DateTime topicImageDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual SocialBlog SocialBlog { get; set; }



    }


    public class SocialBlogContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int contentID { get; set; }
        public int blogID { get; set; }
        public int position { get; set; }
        public string contentTitle { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public DateTime contentDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual SocialBlog SocialBlog { get; set; }
    }

    public class BlogContentSection
    {
        public string Topic { get; set; }
        public string Content { get; set; }
    }
    public class SocialBlogComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int commentID { get; set; }
        public int blogID { get; set; }
        public int userID { get; set; }
        public string commentContent { get; set; }
        public string commentStatus { get; set; }
        public int commentLike { get; set; }
        //public string commentImage { get; set; }
        public DateTime commentDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual SocialBlog SocialBlog { get; set; }

    }
    public class SocialMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int messageID { get; set; }
        public int senderID { get; set; }
        public int receiverID { get; set; }
        public int groupID { get; set; }
        public string messageContent { get; set; }
        public string messageStatus { get; set; }
        public DateTime messageDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual SocialGroup SocialGroup { get; set; }
        //public virtual ICollection<SocialSrcMessage> SocialSrcMessages { get; set; }
    } 
    public class SocialGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int groupID { get; set; }
        public string groupName { get; set; }
        public string groupDescription { get; set; }
        public string groupStatus { get; set; }
        public int srcID { get; set; }
        public DateTime groupDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual ICollection<SocialMessage> SocialMessages { get; set; }
        public virtual ICollection<SocialGroupUser> SocialGroupUsers { get; set; }
    }
    public class SocialGroupUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int socialGroupUserID { get; set; }
        public int groupID { get; set; }
        public int userID { get; set; }
        public DateTime memberDate { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual SocialGroup SocialGroup { get; set; }
    }
}
