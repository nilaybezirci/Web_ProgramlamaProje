using Eczane.Data;
using Eczane.Models;
using Eczane.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eczane.Controllers
{


    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser { UserName = user.UserName, Email = user.Email, Address = user.Address, LastName = user.LastName, Name = user.Name };
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    return Redirect("/drugs");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(user);
        }

        public IActionResult Login(string? returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

     

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    if (loginVM.UserName == "Admin")
                    {
                        return Redirect(loginVM.ReturnUrl ?? "/");
                    }
                    else
                    {
                        return Redirect(loginVM.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("", "Invalid username or password");
            }

            return View(loginVM);
        }

        public async Task<IActionResult> UpdateUser()
        {
            if (User.Identity != null)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                return View(user);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(AppUser appUser)
        {

            var user = _userManager.GetUserAsync(User).Result;
            user.Name = appUser.Name;
            user.LastName = appUser.LastName;
            user.Address = appUser.Address;

          

            await _userManager.UpdateAsync(user);
            TempData["Success"] = "User information updated!";

            return Redirect("/home");
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();

            return Redirect(returnUrl);
        }
    }
}

