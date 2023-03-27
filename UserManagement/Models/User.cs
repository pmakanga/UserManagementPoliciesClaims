using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
      //  public int? UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
    }
}
