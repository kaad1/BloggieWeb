using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BloggieWeb1.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRroleId = "7a60c941-4139-4c38-b930-49d08809ae5d";
            var superAdminRoleId = "51a78bd9-fee4-43a4-9d63-4100e0f91cf0";
            var userRoleId = "0b651ea1-8f42-44a3-a44e-97db1562bf91";
            // Seed Roles /User, Admin, SuperAdmin

            //Ktu i kena kriju rolet mas pari si admin edhe super admin 
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                  Name="Admin",
                  NormalizedName="Admin",
                  Id=adminRroleId,
                  ConcurrencyStamp=adminRroleId
                },
                new IdentityRole
                {
                  Name = "SuperAdmin",
                  NormalizedName = "SuperAdmin",
                  Id = superAdminRoleId,
                  ConcurrencyStamp= superAdminRoleId

                },

                 new IdentityRole
                 {
                   Name = "User",
                   NormalizedName="User",
                   Id=userRoleId,
                   ConcurrencyStamp= userRoleId
                 }

            };




            builder.Entity<IdentityRole>().HasData(roles);




            //Seed SuperAdmin user
            var superAdminId = "9d8842ae-ba9d-4cb1-8403-7b9061d635e7";
            // Ktu i kena kriju userin e superadmin
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@blogie.com",
                Email = "superadmin@blogie.com",
                NormalizedEmail = "superadmin@blogie.com".ToUpper(),
                NormalizedUserName = "superadmin@blogie.com".ToUpper(),
                Id = superAdminId,


            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "SuperAdmin123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add ALL the Roles to SuperAdminUser

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId=adminRroleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId=superAdminRoleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId=userRoleId,
                    UserId=superAdminId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);



        }
    }
}
