using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IToastNotification toastNotification;
        private readonly INotyfService notyfService;

        public UserRolesController(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<IdentityUser> signInManager,
            IToastNotification toastNotification,
            INotyfService notyfService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.toastNotification = toastNotification;
            this.notyfService = notyfService;
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
            //toastNotification.AddSuccessToastMessage("Role Updated Sucesfully!");
            //toastNotification.AddErrorToastMessage("Error Message");
            //toastNotification.AddInfoToastMessage("Information");
            //toastNotification.AddWarningToastMessage("Warning Message!");
            notyfService.Success("Role Updated Sucessfuly!");
            notyfService.Information("Information message!");
            notyfService.Error("Error message!");
            notyfService.Warning("Warning message!");
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

    }
}
