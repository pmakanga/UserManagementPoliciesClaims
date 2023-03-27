using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                var allUsersExceptCurrentUser = await userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
                return View(allUsersExceptCurrentUser);
            }
            catch (Exception ex)
            {
                return null;   
            }
        }
    }
}
