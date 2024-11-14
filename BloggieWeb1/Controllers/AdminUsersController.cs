using BloggieWeb1.Models.Domain.ViewModels;
using BloggieWeb1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BloggieWeb1.Controllers
{
    [Authorize (Roles ="Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;

        public AdminUsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task <IActionResult> List()
        {

            var users = await userRepository.GetAll();

            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.Domain.ViewModels.User
                {
                    Id=Guid.Parse(user.Id), 
                    UserName=user.UserName,
                    EmailAddress=user.Email
                });

               
            }
            return View(usersViewModel);
        }
    }
}
