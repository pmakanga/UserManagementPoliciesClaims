using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Permission
{
    //this class will help to evaluate permission
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
