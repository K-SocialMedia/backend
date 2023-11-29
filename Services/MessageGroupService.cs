using ChatChit.Data;
using ChatChit.Helpers;
using ChatChit.Models.GroupChat;
using ChatChit.Models.RequestModel;
using ChatChit.Models.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Services
{
    public interface IMessageGroupService
    {
        public Task<StatusHelper> AddGroup(GroupWithUserRequestModel groupWithUser);
        public Task<List<GroupWithUserResponseModel>> GetGroupsForUser(Guid userId);
        public Task<List<MessageGroupResponseModel>> GetGroupMessages(Guid currentUserId, Guid groupId);
    }
    public class MessageGroupService : IMessageGroupService
    {
        private readonly ChatChitContext _context;

        public MessageGroupService(ChatChitContext context)
        {
            _context = context;
        }

        public async Task<StatusHelper> AddGroup(GroupWithUserRequestModel groupWithUser)
        {
            if (groupWithUser == null || string.IsNullOrEmpty(groupWithUser.groupName)
                || groupWithUser.usersId == null || groupWithUser.usersId.Length == 0)
            {
                return new StatusHelper { isSuccess = false, message = "Có lỗi xảy ra" };
            }

            var newGroup = new GroupChatModel
            {
                id = Guid.NewGuid(),
                groupName = groupWithUser.groupName
            };
            _context.GroupChats.Add(newGroup);
            await _context.SaveChangesAsync();

            foreach (var userId in groupWithUser.usersId)
            {
                var groupMember = new GroupChatMemberModel
                {
                    userId = userId,
                    groupId = newGroup.id
                };
                _context.GroupChatMembers.Add(groupMember);
            }
            await _context.SaveChangesAsync();

            return new StatusHelper { isSuccess = true, message = "Tạo nhóm thành công" };
        }

        public async Task<List<GroupWithUserResponseModel>> GetGroupsForUser(Guid userId)
        {
            var userGroups = await _context.GroupChatMembers
       .Where(gcm => gcm.userId == userId)
       .Select(gcm => gcm.groupId)
       .ToListAsync();

            var groups = await _context.GroupChats
                .Where(gc => userGroups.Contains(gc.id))
                .ToListAsync();

            var groupDetails = new List<GroupWithUserResponseModel>();

            foreach (var group in groups)
            {
                var groupMembers = await _context.GroupChatMembers
                    .Where(gcm => gcm.groupId == group.id)
                    .Select(gcm => gcm.userId)
                    .ToListAsync();

                var users = await _context.Users
                    .Where(u => groupMembers.Contains(u.id))
                    .ToListAsync();

                var userModels = users.Select(u => new UserResponseModel2
                {
                    id = u.id,
                    fullName = u.fullName,
                    image = u.image,
                    nickName = u.nickName,
                    email = u.email
                }).ToList();

                var groupDetail = new GroupWithUserResponseModel
                {
                    id = group.id,
                    groupName = group.groupName,
                    users = userModels
                };

                groupDetails.Add(groupDetail);
            }

            return groupDetails;
        }

        public async Task<List<MessageGroupResponseModel>> GetGroupMessages(Guid currentUserId, Guid groupId)
        {
            var group = await _context.GroupChats.FindAsync(groupId);

            if (group == null)
            {
                return null;
            }

            var rawMessages = await _context.GroupChatMessages
                .Where(m => m.groupId == groupId)
                .OrderBy(m => m.createAt)
                .ToListAsync();

            var messages = rawMessages.TakeLast(10).OrderBy(m => m.createAt).ToList();

            var messageResponseList = new List<MessageGroupResponseModel>();

            foreach (var message in messages)
            {
                var sender = await _context.Users.FindAsync(message.senderId);

                if (sender != null)
                {
                    var messageResponse = new MessageGroupResponseModel
                    {
                        id = message.id,
                        senderId = message.senderId,
                        senderName = sender.nickName,
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
