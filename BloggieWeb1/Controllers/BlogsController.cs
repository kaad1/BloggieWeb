using BloggieWeb1.Models.Domain.ViewModels;
using BloggieWeb1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Metadata;

namespace BloggieWeb1.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRespository blogPostRespository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        public BlogsController(IBlogPostRespository blogPostRespository, IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostRespository = blogPostRespository;
            this.blogPostLikeRepository = blogPostLikeRepository;
        }

        public IBlogPostLikeRepository BlogPostLikeRepository { get; }

        [HttpGet]
        public async Task <IActionResult> Index( string urlHandle)
        {
           var blogPost= await blogPostRespository.GetByUrlHandleAsync(urlHandle);
        
            var blogDetailsViewModel = new BlogDetailsViewModel();
           

            if (blogPost != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);
            
                blogDetailsViewModel = new BlogDetailsViewModel           
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes



                };

            }
            return View(blogDetailsViewModel);
         }
    }
}
