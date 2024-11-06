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

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await _bloggieDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null) {

                _bloggieDbContext.BlogPosts.Remove(existingBlog);
                await _bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await _bloggieDbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {

          return await _bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
          return  await _bloggieDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x=> x.UrlHandle== urlHandle);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {

           var existingTag= await _bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingTag != null) 
            {
                existingTag.Id= blogPost.Id;
                existingTag.Heading = blogPost.Heading;
                existingTag.PageTitle = blogPost.PageTitle;
                existingTag.Content = blogPost.Content;
                existingTag.ShortDescription = blogPost.ShortDescription;
                existingTag.Author = blogPost.Author;
                existingTag.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingTag.UrlHandle = blogPost.UrlHandle;
                existingTag.Visible = blogPost.Visible;
                existingTag.Tags = blogPost.Tags;
                existingTag.PublishedDate = blogPost.PublishedDate;

                await _bloggieDbContext.SaveChangesAsync();
                return existingTag;

            }
            return null;
        
        }

        
    }
}
