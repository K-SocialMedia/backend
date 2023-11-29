using ChatChit.Data;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using ChatChit.Models.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Services
{
    public interface ICommentService
    {
        public Task<List<CommentResponseModel>> GetComment(Guid postId);
        public Task<CommentResponseModel> AddComment(Guid currentUserId, CommentRequestModel model);
    }
    public class CommentService : ICommentService
    {
        private readonly ChatChitContext _context;

        public CommentService(ChatChitContext context)
        {
            _context = context;
        }
        public async Task<List<CommentResponseModel>> GetComment(Guid postId)
        {
            var comments = await _context.Comments
               .Where(c => c.postId == postId)
               .Include(c => c.User)
               .Select(c => new CommentResponseModel
               {
                   content = c.content,
                   ownerName = c.User.nickName,
                   createdAt = c.createdAt
               })
               .ToListAsync();

            return comments;
        }
        public async Task<CommentResponseModel> AddComment(Guid currentUserId, CommentRequestModel model)
        {
            var post = await _context.Posts.FindAsync(model.postId);
            if (post == null)
            {
                return null;
            }
            var user = await _context.Users.FindAsync(currentUserId);
            if (user == null)
            {
                return null;
            }

            CommentModel comment = new CommentModel
            {
                ownerId = currentUserId,
                postId = post.id,
                content = model.content,
                createdAt = DateTime.UtcNow,
                updatedAt = DateTime.UtcNow
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return new CommentResponseModel
            {
                content = comment.content,
                ownerName = user.nickName,
                createdAt = comment.updatedAt,
            };
        }
    }
}
