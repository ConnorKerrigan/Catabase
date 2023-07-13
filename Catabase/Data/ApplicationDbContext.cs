using Catabase.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Catabase.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CatabaseUser> CatabaseUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Cat> Cats { get; set; } 
        public DbSet<PostAttribution> PostAttributions { get; set;}
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Profile>()
                .HasOne(a => a.User)
                .WithOne(b => b.Profile)
                .HasForeignKey<Profile>(a => a.UserId);
        }
    }
}