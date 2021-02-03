using BookReadingEventManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookReadingEventManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Events> BookReadingEvents { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //For the UsersProject Class
            builder.Entity<UsersEvents>().HasKey(UserEvent => new { UserEvent.UserId, UserEvent.EventId });
            builder.Entity<UsersEvents>()
                .HasOne<ApplicationUser>(a => a.Users)
                .WithMany(a => a.UsersEventses)
                .HasForeignKey(a => a.UserId);
            builder.Entity<UsersEvents>()
                .HasOne<Events>(a => a.Events)
                .WithMany(a => a.UsersEventses)
                .HasForeignKey(a => a.EventId);
        }
    }
}
