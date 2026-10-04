using System;
using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Users.Infrastructures.Persistance
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<projekt_inzynierski.Server.Users.Domain.Models.Admin> Admins { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<projekt_inzynierski.Server.Users.Domain.Models.Admin>().HasData(
              new projekt_inzynierski.Server.Users.Domain.Models.Admin
              {
                  Id = 1,
                  Name = "admin",
                  Login = "",
                  Password = "" 
              }
          );

            

                        modelBuilder.Entity<User>()
       .HasKey(u => u.Id);


            modelBuilder.Entity<User>()
    .HasIndex(u => u.Email)
    .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PublicId)
                .IsUnique(); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserPublicId)
                .HasPrincipalKey(u => u.PublicId);


          
        }
    }
}