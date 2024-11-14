using BloggieWeb1.Data;
using BloggieWeb1.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BloggieWeb1.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await bloggieDbContext.BlogPostComment.AddAsync(blogPostComment);
            await bloggieDbContext.SaveChangesAsync();  

            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
           return await bloggieDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId)
           .ToListAsync();
        }
    }
}
