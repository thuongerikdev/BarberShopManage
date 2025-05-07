using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.Domain
{
    [Table(nameof(SocialSrc), Schema = BM.Constant.Database.DbSchema.Social)]
    public class SocialSrc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int srcID { get; set; }
        [MaxLength(50)]
        public string srcName { get; set; }
        public string srcTitle { get; set; }

        public string imageSrc { get; set; }
        public DateTime srcDate { get; set; } = DateTime.Now;




        //public virtual ICollection<SocialSrcBlog> SocialSrcBlogs { get; set; }
        //public virtual ICollection<SocialSrcMessage> SocialSrcMessages { get; set; }
    }
    //public class SocialSrcBlog
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int srcBlogID { get; set; }
    //    public int srcID { get; set; }
    //    public int blogID { get; set; }
    //    public int position { get; set; }
    //    public DateTime srcBlogDate { get; set; } = DateTime.Now;
    //    public virtual SocialBlog SocialBlog { get; set; }
    //    public virtual SocialSrc SocialSrc { get; set; }

    //}
    //public class  SocialSrcMessage 
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int srcMessageID { get; set; }
    //    public int srcID { get; set; }
    //    public int messageID { get; set; }
    //    public DateTime srcMessageDate { get; set; } = DateTime.Now;
    //    public virtual SocialMessage SocialMessage { get; set; }
    //    public virtual SocialSrc SocialSrc { get; set; }
    //}
    
    //public class SocialSrcUser
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int scrUserID { get; set; }
    //    public int srcID { get; set; }
    //    public int userID { get; set; }
    //}
}