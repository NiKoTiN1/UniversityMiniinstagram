using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<PostReport> PostReports { get; set; }
        public DbSet<CommentReport> CommentReports { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                    .Entity<Image>().HasData(new Image()
                    {
                        Id = new Guid().ToString(),
                        Path = "/Images/noPhoto.png",
                        UploadDate = DateTime.UtcNow
                    });

            modelBuilder
                .Entity<PostReport>()
                .HasOne(e => e.Post)
                .WithOne(e => e.Report)
                .HasForeignKey<PostReport>(key => key.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<CommentReport>()
                .HasOne(e => e.Comment)
                .WithOne(e => e.Report)
                .HasForeignKey<CommentReport>(key => key.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Image>()
                .HasOne(e => e.Post)
                .WithOne(e => e.Image)
                .HasForeignKey<Image>(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Comment>()
                .HasOne(e => e.Post)
                .WithMany(e => e.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Like>()
                .HasOne(e => e.Post)
                .WithMany(e => e.Likes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostReport>().HasIndex(e => e.PostId).IsUnique(false);
            modelBuilder.Entity<CommentReport>().HasIndex(e => e.CommentId).IsUnique(false);
            modelBuilder.Entity<CommentReport>().HasIndex(e => e.CommentId).IsUnique(false);

            modelBuilder
                .Entity<Image>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<Post>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<ApplicationUser>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<Comment>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<Like>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<PostReport>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
