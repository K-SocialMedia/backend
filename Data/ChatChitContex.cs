using ChatChit.Models;
using ChatChit.Models.GroupChat;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Data
{
    public class ChatChitContex : DbContext
    {
        public ChatChitContex(DbContextOptions<ChatChitContex> options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<FriendModel> Friends { get; set; }
        public DbSet<TokenModel> Tokens { get; set; }
        //public DbSet<MessageModel> Messages { get; set; }
        //public DbSet<GroupChatMemberModel> GroupChatMembers { get; set; }
        //public DbSet<GroupChatMessageModel> GroupChatMessages { get; set; }
        //public DbSet<GroupChatModel> groupChats { get; set; }
    }
}
