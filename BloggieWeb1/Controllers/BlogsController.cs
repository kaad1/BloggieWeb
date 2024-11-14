using BloggieWeb1.Models.Domain;
using BloggieWeb1.Models.Domain.ViewModels;
using BloggieWeb1.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Metadata;

namespace BloggieWeb1.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRespository blogPostRespository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRespository blogPostRespository, IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRespository = blogPostRespository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        public IBlogPostLikeRepository BlogPostLikeRepository { get; }

        [HttpGet]
        public async Task <IActionResult> Index( string urlHandle)
        {
           var liked = false;
           var blogPost= await blogPostRespository.GetByUrlHandleAsync(urlHandle);
        
            var blogDetailsViewModel = new BlogDetailsViewModel();
           

            if (blogPost != null)
            {
             
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);
                if (signInManager.IsSignedIn(User))
                {
                    //Get like for this blog fot this user
                    var likeForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);
                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                      var likeFromUser= likeForBlog.FirstOrDefault(x=>x.UserId==Guid.Parse(userId));
                      liked=likeFromUser != null;
                    }
             
                }
                //Get comments for blogPost

                var blogCommentsDomainModel= await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);
           
                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel) 
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName

                    });
                }
              

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
                    TotalLikes = totalLikes,
                    Liked=liked,
                    CommentDescription=blogCommentsForView


                };

            }
            return View(blogDetailsViewModel);
         }

        [HttpPost]
        public async Task <IActionResult> Index( BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.ShortDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded=DateTime.Now
                };
 
               await  blogPostCommentRepository.AddAsync(domainModel);
               return RedirectToAction("Index", "Blogs",
               new { urlHandle=blogDetailsViewModel.UrlHandle });
            }

            return View();
           
        }
    }
}
