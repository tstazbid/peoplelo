using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Peoplelo.Data;
using Peoplelo.Models;

namespace Peoplelo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbContext _context;
        private readonly SignInManager<Users> signInManager;

        public HomeController(ILogger<HomeController> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            // fetch users from the database and sort by LastVisit
            var users = _context.Users
                .OrderByDescending(u => u.LastVisit)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    LastVisit = u.LastVisit.HasValue ? u.LastVisit.Value.ToString("g") : "N/A",
                    u.Status
                })
                .ToList();

            // pass the user data to the view
            return View(users);
        }


        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // action to block users
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BlockUsers([FromBody] List<string> userIds)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usersToBlock = _context.Users.Where(u => userIds.Contains(u.Id)).ToList();

            if (!usersToBlock.Any())
            {
                return Json(new { success = false, message = "No valid users found to block." });
            }

            // check if the current user is being blocked
            bool isCurrentUserBeingBlocked = usersToBlock.Any(u => u.Id == currentUserId);

            // update the status of the users
            foreach (var user in usersToBlock)
            {
                user.Status = "Blocked";
            }

            await _context.SaveChangesAsync();

            // if current user is blocked , logout and go to login 
            if (isCurrentUserBeingBlocked)
            {

                return Json(new { success = true, redirectToLogin = true });
            }
            return Json(new { success = true, message = "Users have been blocked." });
        }


        // action to unblock users
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnblockUsers([FromBody] List<string> userIds)
        {
            var users = _context.Users.Where(u => userIds.Contains(u.Id));
            foreach (var user in users)
            {
                user.Status = "Active";
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Users have been unblocked." });
        }

        // action to delete users
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteUsers([FromBody] List<string> userIds)
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usersToDelete = _context.Users.Where(u => userIds.Contains(u.Id)).ToList();

            if (!usersToDelete.Any())
            {
                return Json(new { success = false, message = "No valid users found to delete." });
            }

            // check if the current user is being deleted
            bool isCurrentUserDeletingSelf = usersToDelete.Any(u => u.Id == currentUserId);

            // remove users
            _context.Users.RemoveRange(usersToDelete);
            await _context.SaveChangesAsync();

            // sign out the current user if they deleted themselves
            if (isCurrentUserDeletingSelf)
            {
                //await signInManager.SignOutAsync();
                return Json(new { success = true, redirectToLogin = true });
            }

            // Return success for other cases
            return Json(new { success = true });
        }

    }
}
