using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.Dtos
{
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

}
