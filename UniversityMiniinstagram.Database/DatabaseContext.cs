using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityMiniinstagram.Database.Models;

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
                .HasOne(e => e.Post)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Report>()
                .HasOne(e => e.Comment)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Report>().HasIndex(e => e.PostId).IsUnique(false);
            modelBuilder.Entity<Report>().HasIndex(e => e.CommentId).IsUnique(false);

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
                .Entity<Report>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<RolesBeforeBan>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
