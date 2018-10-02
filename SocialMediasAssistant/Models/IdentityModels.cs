using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialMediasAssistant.Common;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SocialMediasAssistant.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual Collection<Content> Contents { get; set; }
        public virtual Collection<AccessToken> AccessTokens { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }
        [DefaultValue(10)]
        [Display(Name = "Points", ResourceType = typeof(Resources.Resource))]
        public int Points { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Content> Contents { get; set; }
        public DbSet<FacebookPostPage> FacebookPostPages { get; set; }
        public DbSet<FacebookPagePost> FacebookPagePosts { get; set; }
        public DbSet<InstagramPost> InstagramPosts { get; set; }
        public DbSet<InstagramAccount> InstagramAccounts { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }

        public IEnumerable<FacebookPostPage> GetFacebookPostPages(string userName)
        {
            return FacebookPostPages.Include("ApplicationUser").Where(c => c.ApplicationUser.UserName != userName)
                .AsEnumerable().OrderByDescending(c =>
            {
                Random random = new Random();
                return random.NextDouble() - c.Points / 100;
            });
        }

        public IEnumerable<FacebookPagePost> GetFacebookPagePosts(string userName)
        {
            return FacebookPagePosts.Include("ApplicationUser").Where(c => c.ApplicationUser.UserName != userName).AsEnumerable().OrderByDescending(c =>
            {
                Random random = new Random();
                return random.NextDouble() - c.Points / 100;
            });
        }

        public IEnumerable<InstagramPost> GetInstagramPosts(string userName)
        {
            return InstagramPosts.Include("ApplicationUser").Where(c => c.ApplicationUser.UserName != userName).AsEnumerable().OrderByDescending(c =>
            {
                Random random = new Random();
                return random.NextDouble() - c.Points / 100;
            });
        }

        public IEnumerable<InstagramAccount> GetInstagramAccounts(string userName)
        {
            return InstagramAccounts.Include("ApplicationUser").Where(c => c.ApplicationUser.UserName != userName).AsEnumerable().OrderByDescending(c =>
            {
                Random random = new Random();
                return random.NextDouble() - c.Points / 100;
            });
        }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                .Ignore(m => m.EmailConfirmed)
                .Ignore(m => m.PasswordHash)
                .Ignore(m => m.PhoneNumber)
                .Ignore(m => m.PhoneNumberConfirmed)
                .Ignore(m => m.TwoFactorEnabled)
                .Ignore(m => m.LockoutEndDateUtc)
                .Ignore(m => m.LockoutEnabled)
                .Ignore(m => m.AccessFailedCount);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}