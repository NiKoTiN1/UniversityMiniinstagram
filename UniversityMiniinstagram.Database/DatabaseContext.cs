using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using UniversityMiniinstagram.Database.Models;
using System.Linq;

namespace UniversityMiniinstagram.Database
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Image> Images { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }

        public DbSet<RolesBeforeBan> RolesBeforeBan { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Report>()
                .HasOne<Post>(e => e.Post)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
