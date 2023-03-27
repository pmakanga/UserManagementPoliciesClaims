using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Constants;
using UserManagement.Helpers;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class PermissionController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public PermissionController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Products), roleId);
            var role = await roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            var claims = await roleManager.GetClaimsAsync(role);
            var allClaimsValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimsValues.Intersect(roleClaimValues).ToList();
            foreach (var permissions in allPermissions)
            {
                if(authorizedClaims.Any(a => a==permissions.Value))
                {
                    permissions.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;

            return View(model);
        }

        public async Task<IActionResult>Update(PermissionViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.RoleId);
            var claims = await roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await roleManager.RemoveClaimAsync(role, claim);
            }

            var selectedClaims = model.RoleClaims?.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await roleManager.AddPermissionClaim(role, claim.Value);
            }
            
            return RedirectToAction("Index", new { roleId = model.RoleId});
        }

    }
}
