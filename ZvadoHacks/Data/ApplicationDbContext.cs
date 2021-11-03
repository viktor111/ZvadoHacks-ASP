using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Image)
                .WithOne(i => i.Article)
                .HasForeignKey<ImageData>(i => i.ArticleId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.ProfilePicture)
                .WithOne(i => i.User)
                .HasForeignKey<ImageData>(i => i.UserId);

            modelBuilder.Entity<ProjectData>()
                .HasOne(p => p.Image)
                .WithOne(i => i.Project)
                .HasForeignKey<ImageData>(i => i.ProjectDataId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Article>()
                .HasMany(a => a.Comments)
                .WithOne(c => c.Article)
                .HasForeignKey(c => c.ArticleId);            
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ImageData> Images { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ProjectData> ProjectsData { get; set; }

        public DbSet<AboutMe> AboutMe { get; set; }
    }
}
