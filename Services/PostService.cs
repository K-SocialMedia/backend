using ChatChit.Data;
using ChatChit.Models.Post;
using ChatChit.Models.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Services
{
    public interface IPostService
    {
        public Task<List<PostModel>> GetAllPostById(Guid id);
        public Task<PostModel> AddPost(Guid id, PostRequestModel post);
        public Task<bool> DeletePost(Guid currentUserId, Guid postId);
    }
    public class PostService:IPostService
    {
        private readonly ChatChitContext _context;
        public PostService(ChatChitContext context)
        {
            _context = context;
        }
        public async Task<List<PostModel>> GetAllPostById(Guid id)
        {
            var posts = await _context.Posts.Where(p => p.ownerId == id).ToListAsync();
            return posts;
        }

        public async Task<PostModel> AddPost(Guid id, PostRequestModel post)
        {
            PostModel postModel = new PostModel();
            postModel.ownerId = id;
            postModel.content = post.content;
            postModel.image = post.image;
            postModel.createdAt = DateTime.UtcNow;
            postModel.updatedAt = DateTime.UtcNow;
            _context.Posts.Add(postModel);
            await _context.SaveChangesAsync();
            return postModel;
        }

        public async Task<bool> DeletePost(Guid currentUserId, Guid postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if(post != null)
            {
                if(post.ownerId == currentUserId)
                {
                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
