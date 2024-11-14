using BloggieWeb1.Models.Domain;

namespace BloggieWeb1.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
    }
}
