﻿using BloggieWeb1.Models.Domain;

namespace BloggieWeb1.Repositories
{
    public interface IBlogPostRespository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetAsync(Guid id);
        Task <BlogPost?> GetByUrlHandleAsync(string urlHandle);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);

    }
}
