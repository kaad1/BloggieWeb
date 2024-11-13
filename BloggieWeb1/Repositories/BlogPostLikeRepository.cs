﻿
using BloggieWeb1.Data;
using BloggieWeb1.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BloggieWeb1.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await bloggieDbContext.BlogPostLike.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();  
            return blogPostLike;
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
          return await bloggieDbContext.BlogPostLike
                .CountAsync(x=>x.BlogPostId==blogPostId); 
        }
    }
}
