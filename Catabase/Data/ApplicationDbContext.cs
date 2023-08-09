using Catabase.Models;
using Microsoft.AspNetCore.Identity;
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

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "1d4defef-a40d-41ef-a329-3a141369652e", Name = "Admin", NormalizedName = "ADMIN".ToUpper() });

            var hasher = new PasswordHasher<CatabaseUser>();


            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<CatabaseUser>().HasData(
                new CatabaseUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    PasswordHash = hasher.HashPassword(null, "Admin")
                }
            );

            modelBuilder.Entity<Profile>().HasData(
                new Profile
                {
                    ProfileId = -1, // primary key
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    
                }
            );


            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1d4defef-a40d-41ef-a329-3a141369652e",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );
        }
    }
}