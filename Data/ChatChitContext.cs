﻿using ChatChit.Models;
using ChatChit.Models.GroupChat;
using ChatChit.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Data
{
    public class ChatChitContext : DbContext
    {
        public ChatChitContext(DbContextOptions<ChatChitContext> options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<FriendModel> Friends { get; set; }
        public DbSet<TokenModel> Tokens { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<GroupChatModel> GroupChats { get; set; }
        public DbSet<GroupChatMessageModel> GroupChatMessages { get; set; }
        public DbSet<GroupChatMemberModel> GroupChatMembers { get; set; }
    }
}