using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Courses.Infrastructures.Persistance;
using projekt_inzynierski.Server.PremiumStore.Domain.Models;

namespace projekt_inzynierski.Server.PremiumStore.Infrastructures.Persistance
{
    public class StoreDbContext : DbContext
    {

        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<UserAvatar> UserAvatars { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<UserReward> UserRewards { get; set; }
        public DbSet<AvatarReward> AvatarRewards { get; set; }
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Avatar>().HasData(
                new Avatar
                {

                    IsActive = true,
                    Id = 1,
                    ImageUrl = "Images/blade-runner.png",
                    Name = "Łowca androidów"
                },
                new Avatar
                {

                    IsActive = true,
                    Id = 2,
                    ImageUrl = "Images/clockwork-orange.jpg",
                    Name = "Nakręcony człowiek"
                },

                new Avatar
                {

                    IsActive = true,
                    Id = 3,
                    ImageUrl = "Images/hal-9000.png",
                    Name = "Nieomylne AI"
                },
                new Avatar
                {
                    IsActive = true,
                    Id = 4,
                    ImageUrl = "Images/interstellar.jpg",
                    Name = "Podróż w nieznane"
                },
                new Avatar
                {

                    IsActive = true,
                    Id = 5,
                    ImageUrl = "Images/moon.webp",
                    Name = "Nie jesteś sam"
                },
                new Avatar
                {

                    IsActive = true,
                    Id = 6,
                    ImageUrl = "Images/tehc.jpg",
                    Name = "Tech"
                },
                new Avatar
                {

                    IsActive = true,
                    Id = 7,
                    ImageUrl = "Images/trip.webp",
                    Name = "Memodestruktor"
                },
                new Avatar
                {

                    IsActive = true,
                    Id = 8,
                    ImageUrl = "Images/default-avatar.jpg",
                    Name = "Memodestruktor"
                },
                 new Avatar
                 {

                     IsActive = true,
                     Id = 9,
                     ImageUrl = "Images/water.webp",
                     Name = "Wodna planeta"
                 },
                 new Avatar
                 {

                     IsActive = true,
                     Id = 10,
                     ImageUrl = "Images/wide.webp",
                     Name = "Oczy zamkniete"
                 },
                 new Avatar
                 {

                     IsActive = true,
                     Id = 11,
                     ImageUrl = "Images/space.jpg",
                     Name = "W nadprzestrzen"
                 },
                 new Avatar
                 {

                     IsActive = true,
                     Id = 12,
                     ImageUrl = "Images/turnhal.jpg",
                     Name = "Wylaczenie"
                 },
                 new Avatar
                 {

                     IsActive = true,
                     Id = 13,
                     ImageUrl = "Images/truman.webp",
                     Name = "Prawda"
                 },
                 new Avatar
                 {

                     IsActive = true,
                     Id = 14,
                     ImageUrl = "Images/vendetta.jpg",
                     Name = "Prawda"
                 }
                );


            modelBuilder.Entity<Reward>().HasData(
                new Reward
                {
                    Cost = 150,
                    IsActive = true,
                    Description = "fajny obrazek",
                    Id = 1,
                    ImageUrl = "Images/blade-runner.png",
                    Name = "Awatar",
                    Type = "Awatar"
                },
                  new Reward
                  {
                      Cost = 100,
                      IsActive = true,
                      Description = "fajny obrazek",
                      Id = 2,
                      ImageUrl = "Images/clockwork-orange.jpg",
                      Name = "Awatar",
                      Type = "Awatar"
                  },
                   new Reward
                   {
                       Cost = 100,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 3,
                       ImageUrl = "Images/hal-9000.png",
                       Name = "Awatar",
                       Type = "Awatar"
                   },
                  new Reward
                  {
                      Cost = 110,
                      IsActive = true,
                      Description = "fajny obrazek",
                      Id = 4,
                      ImageUrl = "Images/interstellar.jpg",
                      Name = "Awatar",
                      Type = "Awatar"
                  },
                   new Reward
                   {
                       Cost = 200,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 5,
                       ImageUrl = "Images/moon.webp",
                       Name = "Awatar",
                       Type = "Awatar"
                   },
                  new Reward
                  {
                      Cost = 50,
                      IsActive = true,
                      Description = "fajny obrazek",
                      Id = 6,
                      ImageUrl = "Images/tehc.jpg",
                      Name = "Awatar",
                      Type = "Awatar"
                  },
                   new Reward
                   {
                       Cost = 100,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 7,
                       ImageUrl = "Images/trip.webp",
                       Name = "Awatar",
                       Type = "Awatar"
                   }
                   ,
                   new Reward
                   {
                       Cost = 150,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 8,
                       ImageUrl = "Images/water.webp",
                       Name = "Awatar",
                       Type = "Awatar"
                   },
                   new Reward
                   {
                       Cost = 250,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 9,
                       ImageUrl = "Images/wide.webp",
                       Name = "Awatar",
                       Type = "Awatar"
                   },
                   new Reward
                   {
                       Cost = 150,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 10,
                       ImageUrl = "Images/space2001.jpg",
                       Name = "Awatar",
                       Type = "Awatar"
                   },
                   new Reward
                   {
                       Cost = 350,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 11,
                       ImageUrl = "Images/turnhal.jpg",
                       Name = "Awatar",
                       Type = "Awatar"
                   },
                   new Reward
                   {
                       Cost = 450,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 12,
                       ImageUrl = "Images/truman.webp",
                       Name = "Awatar",
                       Type = "Awatar"
                   },
                   new Reward
                   {
                       Cost = 100,
                       IsActive = true,
                       Description = "fajny obrazek",
                       Id = 13,
                       ImageUrl = "Images/vendetta.jpg",
                       Name = "Awatar",
                       Type = "Awatar"
                   },
                   new Reward
                   {
                       Cost = 500,
                       IsActive = true,
                       Description = "Zyskaj dodatkowe 100% PD",
                       Id = 14,
                       ImageUrl = "Images/newPremium1.png",
                       Name = "Konto premium na dzień",
                       Type = "Premium1"
                   },
                   new Reward
                   {
                       Cost = 500,
                       IsActive = true,
                       Description = "Zyskaj dodatkowe 100% PD",
                       Id = 15,
                       ImageUrl = "Images/newPremium7.png",
                       Name = "Konto premium na tydzień",
                       Type = "Premium7"
                   },
                   new Reward
                   {
                       Cost = 500,
                       IsActive = true,
                       Description = "Zyskaj dodatkowe 100% PD",
                       Id = 16,
                       ImageUrl = "Images/newPremium30.png",
                       Name = "Konto premium na miesiąc",
                       Type = "Premium30"
                   }
                );

            modelBuilder.Entity<AvatarReward>().HasData(
                new AvatarReward
                {
                    Id = 1,
                    AvatarId = 1,
                    RewardId = 1,
                },
                  new AvatarReward
                  {
                      Id = 2,
                      AvatarId = 2,
                      RewardId = 2,
                  },
                  new AvatarReward
                  {
                      Id = 3,
                      AvatarId = 3,
                      RewardId = 3,
                  },
                 new AvatarReward
                 {
                     Id = 4,
                     AvatarId = 4,
                     RewardId = 4,
                 },
                   new AvatarReward
                   {
                       Id = 5,
                       AvatarId = 5,
                       RewardId = 5,
                   },
                  new AvatarReward
                  {
                      Id = 6,
                      AvatarId = 6,
                      RewardId = 6,
                  },
                  new AvatarReward
                  {
                      Id = 7,
                      AvatarId = 7,
                      RewardId = 7,
                  }
                  ,
                  new AvatarReward
                  {
                      Id = 8,
                      AvatarId = 9,
                      RewardId = 8,
                  },
                  new AvatarReward
                  {
                      Id = 9,
                      AvatarId = 10,
                      RewardId = 9,
                  },
                  new AvatarReward
                  {
                      Id = 10,
                      AvatarId = 11,
                      RewardId = 10,
                  },
                  new AvatarReward
                  {
                      Id = 11,
                      AvatarId = 12,
                      RewardId = 11,
                  },
                  new AvatarReward
                  {
                      Id = 12,
                      AvatarId = 13,
                      RewardId = 12,
                  },
                  new AvatarReward
                  {
                      Id = 13,
                      AvatarId = 14,
                      RewardId = 13,
                  }
                );


        }
    }
}