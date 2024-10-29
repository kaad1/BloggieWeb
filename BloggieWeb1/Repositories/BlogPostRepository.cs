using Azure;
using BloggieWeb1.Data;
using BloggieWeb1.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace BloggieWeb1.Repositories
{
    public class BlogPostRepository : IBlogPostRespository
    {
        private readonly BloggieDbContext _bloggieDbContext;
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await _bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await _bloggieDbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public Task<BlogPost> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
