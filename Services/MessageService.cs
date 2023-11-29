using ChatChit.Data;
using ChatChit.Models;
using ChatChit.Models.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Services
{
    public interface IMessageService
    {
        public Task<List<MessageResponseModel>> GetMessageNearly(Guid currentUserId, Guid friendId);
    }
    public class MessageService : IMessageService
    {
        private readonly ChatChitContext _context;
        public MessageService(ChatChitContext context)
        {
            _context = context;
        }
        public async Task<List<MessageResponseModel>> GetMessageNearly(Guid currentUserId, Guid friendId)
        {
            var user = await _context.Users.FindAsync(friendId);
            var messages = await _context.Messages
                 .Where(m => (m.senderId == currentUserId && m.receiverId == friendId) ||
                   (m.senderId == friendId && m.receiverId == currentUserId))
                .OrderByDescending(m => m.createAt)
                .Take(5)
                 .ToListAsync();
            var messageResponseList = new List<MessageResponseModel>();

            foreach (var message in messages)
            {
                var receiver = await _context.Users.FindAsync(message.receiverId);
                var sender = await _context.Users.FindAsync(message.senderId);

                if (receiver != null)
                {
                    var messageResponse = new MessageResponseModel
                    {
                        id = message.id,
                        senderId = message.senderId,
                        senderName = sender.nickName,
                        receiverId = message.receiverId,
                        receiverName = receiver.nickName,
                        content = message.content,
                        image = message.image,
                        createAt = message.createAt,
                        isRead = message.isRead,
                    };

                    messageResponseList.Add(messageResponse);
                }
            }
            return messageResponseList;
        }
    }
}
