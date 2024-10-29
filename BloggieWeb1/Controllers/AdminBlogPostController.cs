using BloggieWeb1.Models.Domain;
using BloggieWeb1.Models.Domain.ViewModels;
using BloggieWeb1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloggieWeb1.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRespository _blogPostRespository;
        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRespository blogPostRespository)
        {
            _tagRepository = tagRepository; 
            _blogPostRespository = blogPostRespository; 
        }
        [HttpGet]
       public async Task <IActionResult> Add()
       {
           //get tags from repository
          var tags= await _tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString()})
            }; 
           return View(model);
       }
        [HttpPost]
        public  async Task <IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {

            //Map View Model To Domain Model 
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,

            };

         
            //Map Tags From Selected Tags 
            var selctedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags) 
            {
                var seletedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existringTag = await _tagRepository.GetAsync(seletedTagIdAsGuid);
                if (existringTag != null) 
                {
                    selctedTags.Add(existringTag);
                }
            }

            
            //Maping tags to domain model 
            blogPost.Tags = selctedTags;
            await _blogPostRespository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        public async Task <IActionResult> List()
        {
            var blogPosts= await _blogPostRespository.GetAllAsync();

            return View(blogPosts);
        }
    }
}
