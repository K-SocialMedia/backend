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

                var groupDetail = new GroupWithUserResponseModel
                {
                    id = group.id,
                    groupName = group.groupName,
                    userId = groupMembers
                };

                groupDetails.Add(groupDetail);
            }
            return groupDetails;
        }
    }
}
