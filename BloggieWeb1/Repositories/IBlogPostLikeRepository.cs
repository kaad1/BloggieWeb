﻿using BloggieWeb1.Models.Domain;

namespace BloggieWeb1.Repositories
{
    public interface IBlogPostLikeRepository
    {
       Task <int> GetTotalLikes(Guid blogPostId);

      Task<IEnumerable<BlogPostLike>>  GetLikesForBlog (Guid blogPostId);
       Task <BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
    }
}
