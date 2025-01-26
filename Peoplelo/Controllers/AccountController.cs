using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peoplelo.Data;
using Peoplelo.Models;
using Peoplelo.ViewModels;

namespace Peoplelo.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly UserDbContext _context;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // check user's status is not "Active" (blocked)
                    if (user.Status != "Active")
                    {
                        ModelState.AddModelError("", "Your account is blocked. Please contact support.");
                        return View(model);
                    }

                    var result = await signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        // update LastVisit to the current time
                        user.LastVisit = DateTime.Now;
                        await userManager.UpdateAsync(user);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email or password is incorrect.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVIewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "The email address is already in use.");
                    return View(model);
                }

                // create a new user
                Users users = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                    Status = "Active",
                    LastVisit = DateTime.Now,
                };

                var result = await userManager.CreateAsync(users, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // show only the first error
                    ModelState.AddModelError("", result.Errors.FirstOrDefault()?.Description ?? "An error occurred during registration.");
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "This email is not found!");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
                return View(model);
            }
        }

        public async Task<IActionResult> CheckUserStatus()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null || user.Status != "Active")
            {
                await signInManager.SignOutAsync();

                return RedirectToAction("Login", "Account");
            }

            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                // update the LastVisit
                user.LastVisit = DateTime.Now;
                await userManager.UpdateAsync(user);
            }

            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
