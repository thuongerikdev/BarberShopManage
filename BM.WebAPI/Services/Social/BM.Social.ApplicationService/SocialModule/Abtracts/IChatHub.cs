using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.SocialModule.Abtracts
{
    public interface IChatHub
    {
        public Task SendPrivateMessage(int senderId, int receiverId, string messageContent);
        public Task SendGroupMessage(int senderId, int groupId, string messageContent);
        
    }
}
