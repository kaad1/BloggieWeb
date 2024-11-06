using BloggieWeb1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Metadata;

namespace BloggieWeb1.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRespository blogPostRespository;

        public BlogsController(IBlogPostRespository blogPostRespository)
        {
            this.blogPostRespository = blogPostRespository;
        }
        [HttpGet]
        public async Task <IActionResult> Index( string urlHandle)
        {
           var blogPost= await blogPostRespository.GetByUrlHandleAsync(urlHandle);
            return View(blogPost);
        }
    }
}
