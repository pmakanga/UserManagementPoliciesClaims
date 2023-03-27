using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Index(string userId)
        {
            var viewModel = new List<UserRolesViewModel>();
            var user = await userManager.FindByIdAsync(userId);
            foreach (var role in roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleName = role.Name,
                };
                if(await userManager.IsInRoleAsync(user,role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected =false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var model = new ManageUserRolesViewModel()
            {
                UserId = userId,
                UserRoles = viewModel
            };
            return View(model);
        }

        public async Task<IActionResult> Update(string id, ManageUserRolesViewModel model)
        {
            var user = await userManager.FindByIdAsync(id);
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            result = await userManager.AddToRolesAsync(user, model.UserRoles?.Where(x => x.Selected).Select(y => y.RoleName));
            var currentUser = await userManager.GetUserAsync(User);
            await signInManager.RefreshSignInAsync(currentUser);
            await Seeds.DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);
            return RedirectToAction("Index", new { userId = id });
        }
        //private async Task<List<string>> GetUserRoles(User user)
        //{
        //    return new List<string>(await userManager.GetRolesAsync(user));
        //}

        //public async Task<IActionResult> Manage(string userId)
        //{
        //    ViewBag.userId = userId;
        //    var user = await userManager.FindByIdAsync(userId);

        //    if(user == null)
        //    {
        //        ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
        //        return View("NotFound");
        //    }

        //    ViewBag.UserName = user.UserName;
        //    var model = new List<ManageUserRolesViewModel>();
        //    foreach (var role in roleManager.Roles)
        //    {
        //        var userRolesViewModel = new ManageUserRolesViewModel
        //        {
        //            RoleId = role.Id,
        //            RoleName = role.Name,
        //        };
        //        if(await userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            userRolesViewModel.Selected = true;
        //        }
        //        else
        //        {
        //            userRolesViewModel.Selected = false;
        //        }
        //        model.Add(userRolesViewModel);
        //    }

        //    return View(model);
            
        //}

        //[HttpPost]
        //public async Task<IActionResult>Manage(List<ManageUserRolesViewModel> model, string userId)
        //{
        //    var user = await userManager.FindByIdAsync(userId);
        //    if(user == null)
        //    {
        //        return View("Error");
        //    }

        //    var roles = await userManager.GetRolesAsync(user);
        //    var result = await userManager.RemoveFromRolesAsync(user, roles);

        //    if(!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Cannot remove user existing roles");
        //        return View(model);
        //    }
        //    result = await userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Cannot add selected roles to user");
        //        return View(model);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
