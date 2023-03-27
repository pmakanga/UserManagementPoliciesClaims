﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserManagement.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                 new IdentityRole
                 {
                     Name = "Admin",
                     NormalizedName = "ADMIN"
                 },
                  new IdentityRole
                  {
                      Name = "Moderator",
                      NormalizedName = "MODERATOR"
                  },
                  new IdentityRole
                  {
                      Name = "Basic",
                      NormalizedName = "BASIC"
                  });
        }
    }
}
