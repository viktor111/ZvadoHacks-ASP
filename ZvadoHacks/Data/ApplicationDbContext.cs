using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite();
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

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ImageData> Images { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
