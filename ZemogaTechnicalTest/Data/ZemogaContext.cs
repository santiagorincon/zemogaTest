using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Models;

namespace ZemogaTechnicalTest.Data
{
    public class ZemogaContext : DbContext
    {
        public ZemogaContext(DbContextOptions<ZemogaContext> options) : base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<PostActivity> PostActivities { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region DB TABLE NAMES DEFINITION
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<PostActivity>().ToTable("PostActivity");
            #endregion

            #region Table properties
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.Property(p => p.ID).ValueGeneratedOnAdd();
                entity.Property(p => p.RoleName).IsRequired();
                entity.Property(p => p.RoleCode).IsRequired();
                entity.Property(p => p.RoleDesc).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.Property(p => p.ID).ValueGeneratedOnAdd();
                entity.HasOne(p => p.Role).WithMany(p => p.Users).HasForeignKey(p => p.RoleID);

                entity.Property(p => p.CreatedDate).IsRequired().HasDefaultValueSql("getdate()");
                entity.Property(p => p.Password).IsRequired();
                entity.Property(p => p.UserFullname).IsRequired();
                entity.Property(p => p.Username).IsRequired();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.Property(p => p.ID).ValueGeneratedOnAdd();

                entity.HasOne(p => p.Author).WithMany(p => p.CreatedPosts).HasForeignKey(p => p.AuthorID);
                entity.HasOne(p => p.Editor).WithMany(p => p.EditedPosts).HasForeignKey(p => p.EditorID);
                entity.Property(p => p.CreatedDate).IsRequired().HasDefaultValueSql("getdate()");
                entity.Property(p => p.PostContent).IsRequired();
                entity.Property(p => p.PostName).IsRequired();
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.Property(p => p.ID).ValueGeneratedOnAdd();
                entity.Property(p => p.StatusName).IsRequired();
                entity.Property(p => p.StatusCode).IsRequired();
                entity.Property(p => p.StatusDesc).IsRequired();
            });

            modelBuilder.Entity<PostActivity>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.Property(p => p.ID).ValueGeneratedOnAdd();

                entity.HasOne(p => p.Post).WithMany(p => p.PostActivities).HasForeignKey(p => p.PostID);
                entity.HasOne(p => p.User).WithMany(p => p.PostActivities).HasForeignKey(p => p.UserID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.OldStatus).WithMany(p => p.OldStatusActivities).HasForeignKey(p => p.OldStatusID);
                entity.HasOne(p => p.NewStatus).WithMany(p => p.NewStatusActivities).HasForeignKey(p => p.NewStatusID);
                entity.Property(p => p.ActivityDate).IsRequired().HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.Property(p => p.ID).ValueGeneratedOnAdd();

                entity.HasOne(p => p.Post).WithMany(p => p.PostComments).HasForeignKey(p => p.PostID);
                entity.HasOne(p => p.User).WithMany(p => p.PostComments).HasForeignKey(p => p.UserID).OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.CreatedDate).IsRequired().HasDefaultValueSql("getdate()");
                entity.Property(p => p.Comment).IsRequired();
            });
            #endregion
        }
    }
}
