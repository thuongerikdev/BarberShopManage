using BM.Social.Domain;
using BM.Social.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SocialDbContext _context;

        public ChatHub(SocialDbContext context)
        {
            _context = context;
        }

        // Gửi tin nhắn riêng tư
        public async Task SendPrivateMessage(int senderId, int receiverId, string messageContent)
        {
            var message = new SocialMessage
            {
                senderID = senderId,
                receiverID = receiverId,
                groupID = 0, // Không thuộc group
                messageContent = messageContent,
                messageStatus = "Sent",
                messageDate = DateTime.Now,
                updateAt = DateTime.Now
            };

            // Lưu vào database
            _context.socialMessages.Add(message);
            await _context.SaveChangesAsync();

            // Gửi tin nhắn tới người nhận
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, messageContent, message.messageID);
        }

        // Gửi tin nhắn nhóm
        public async Task SendGroupMessage(int senderId, int groupId, string messageContent)
        {
            var message = new SocialMessage
            {
                senderID = senderId,
                receiverID = 0, // Không có receiver cá nhân
                groupID = groupId,
                messageContent = messageContent,
                messageStatus = "Sent",
                messageDate = DateTime.Now,
                updateAt = DateTime.Now
            };

            // Lưu vào database
            _context.socialMessages.Add(message);
            await _context.SaveChangesAsync();

            // Lấy danh sách user trong nhóm
            var groupUsers = await _context.socialGroupUsers
                .Where(gu => gu.groupID == groupId)
                .Select(gu => gu.userID.ToString())
                .ToListAsync();

            // Gửi tin nhắn tới tất cả user trong nhóm
            await Clients.Users(groupUsers).SendAsync("ReceiveGroupMessage", senderId, groupId, messageContent, message.messageID);
        }

        // Khi user kết nối
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        // Khi user ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}