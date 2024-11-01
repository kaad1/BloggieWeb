using BloggieWeb1.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net;

namespace BloggieWeb1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
       public  async Task <IActionResult> UploadAsync(IFormFile file)
       {
           //To call a repository
          var imageURL=  await imageRepository.UploadAsync(file);

            if (imageURL == null) 
            {
                return Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError);

            }
            return  new JsonResult(new {link= imageURL});

       }

    }
}
