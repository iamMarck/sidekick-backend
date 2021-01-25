using Microsoft.EntityFrameworkCore;
using SideKick.Examination.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SideKick.Examination.Data
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions options) :
            base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region USER TABLE
            modelBuilder.Entity<User>()
                .Property(s => s.DateCreated)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<User>()
                .HasMany(m => m.UserSaltSessions)
                .WithOne(o => o.User)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region USER SALT SESSION TABLE
            modelBuilder.Entity<UserSaltSession>()
                .Property(s => s.DateCreated)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<UserSaltSession>()
                .HasOne(s => s.User).WithMany()
                .HasForeignKey(u => u.UserId).
                OnDelete(DeleteBehavior.Restrict);
            #endregion

        }


    }
}
