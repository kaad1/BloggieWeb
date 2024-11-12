using BloggieWeb1.Models.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BloggieWeb1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public SignInManager<IdentityUser> signInManager { get; }

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email


            };
          var identityResult=  await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                //Asign this user role

              var roleIdentityResult=  await userManager.AddToRoleAsync(identityUser, "User");

                if (roleIdentityResult.Succeeded) 
                {
                    return RedirectToAction("Register");
                }

            }

            return View("Register"); 
        }

        [HttpGet]
        public async Task <IActionResult> Login()
        {
            return View();
        }
        
        public async Task <IActionResult> Login(LoginViewModel loginViewModel)
        {
          var signInResult= await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
          if(signInResult != null && signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home"); 
            }

           //Show Errors
            return View();
        }

        public async Task <IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]

        public IActionResult AccessDenied()
        {
            return View();  

        }
    }
}
