using BloggieWeb1.Models.Domain;
using BloggieWeb1.Models.Domain.ViewModels;
using BloggieWeb1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;

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
       
        [Authorize(Roles ="Admin")]
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
            var blogPost= await _blogPostRespository.GetAllAsync();

            return View(blogPost);
        }
        [Authorize(Roles = "Admin")]
    
        [HttpGet]
        public  async Task <IActionResult> Edit(Guid id)
        {
           //Retrive the result from repository
           var blogPost=  await _blogPostRespository.GetAsync(id);
           var tagsDomainModel= await _tagRepository.GetAllAsync();

            if (blogPost != null)
            {
              
                var model = new EditBlogPostRequest              
                { 
                    //map the domain model
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                 
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),

                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model); 
            }
               //pas data to view
                 return View(null);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task <IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {

            //map view model back to domain model
            var blogPostDomainModel = new BlogPost
            {
             Id=editBlogPostRequest.Id,
             Heading= editBlogPostRequest.Heading,
             PageTitle= editBlogPostRequest.PageTitle,
             Content = editBlogPostRequest.Content,
             Author = editBlogPostRequest.Author,
             ShortDescription = editBlogPostRequest.ShortDescription,
             FeaturedImageUrl= editBlogPostRequest.FeaturedImageUrl,
             UrlHandle= editBlogPostRequest.UrlHandle,
             PublishedDate= editBlogPostRequest.PublishedDate,
             Visible = editBlogPostRequest.Visible,

            };

            // map tags into domain model 

            var selctedTags = new List<Tag>();

            foreach(var selctedTag in editBlogPostRequest.SelectedTags)
            {
                if(Guid.TryParse(selctedTag, out var tag))
                {
                   var foundTag= await _tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    { 
                        selctedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selctedTags;
            //submit information to repository to update
            var updateBlog= await _blogPostRespository.UpdateAsync(blogPostDomainModel);

            if ( updateBlog != null)
            {
                return RedirectToAction("Edit");
            }

            //Show error notification 
            return RedirectToAction("Edit");
        }

        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            // Talk to respository to delete this blog post and tags 
           var deletedBlogPost= await _blogPostRespository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null) 
            {
                //Show success notification
                return RedirectToAction("List");
            }


            //display the response 
            return RedirectToAction("Edit", new {id=editBlogPostRequest.Id});
        }
    }
}
