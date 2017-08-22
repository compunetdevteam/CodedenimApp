﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodedenimWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            if (authenticationType == "")
            {
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                return userIdentity;

            }
            else
            {
                var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
                return userIdentity;
            }
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            // Add custom user claims here

        }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<CodeninModel.CourseCategory> CourseCategories { get; set; }

        public System.Data.Entity.DbSet<CodeninModel.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<CodeninModel.Module> Modules { get; set; }

        public System.Data.Entity.DbSet<CodeninModel.Quiz.Topic> Topics { get; set; }

        public System.Data.Entity.DbSet<CodeninModel.TopicMaterialUpload> TopicMaterialUploads { get; set; }
    }
}