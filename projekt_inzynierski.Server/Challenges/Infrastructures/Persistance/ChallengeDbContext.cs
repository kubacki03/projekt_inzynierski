using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using projekt_inzynierski.Server.Challenges.Domain.Models;


namespace projekt_inzynierski.Server.Challenges.Infrastructures.Persistance
{
    public class ChallengeDbContext : DbContext
    {
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<WeeklyChallenge> WeeklyChallenges { get; set; }
        public DbSet<UserChallenge> UserChallenges { get; set; }

        public ChallengeDbContext(DbContextOptions<ChallengeDbContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Badge>().HasData(
             new Badge { Id = 1, ImagePath = "klasy_i_obiekty_badge.png", Title = "Mistrz klas" },
              new Badge { Id = 2, ImagePath = "pierwsze_kroki.png", Title = "Pierwsze kroki" },
               new Badge { Id = 3, ImagePath = "for.webp", Title = "Mistrz pętli" },
               new Badge { Id = 4, ImagePath = "pierwsze_kroki_csharp.webp", Title = "Pierwsze kroki C#" },
                new Badge { Id = 5, ImagePath = "listy.webp", Title = "Listy" },
            new Badge { Id = 6, ImagePath = "linq.webp", Title = "Linq" },
            new Badge { Id = 7, ImagePath = "meotdy.jpg", Title = "Metody" },
             new Badge { Id = 8, ImagePath = "dziedziczenie.jpg", Title = "Dziedziczenie" },
           new Badge { Id = 9, ImagePath = "interfejsy.jpg", Title = "Interfejsy" },
            new Badge { Id = 10, ImagePath = "pliki.png", Title = "Pliki" },
           new Badge { Id = 11, ImagePath = "wyjatki.png", Title = "Wyjatki" },
           new Badge { Id = 12, ImagePath = "async.png", Title = "Asynchronicznosc" },
             new Badge { Id = 13, ImagePath = "bazy_danych.png", Title = "Bazy danych" },
             new Badge { Id = 14, ImagePath = "test.jpg", Title = "Testy jednostkowe" },
              new Badge { Id = 15, ImagePath = "rest_api.jpg", Title = "Api Rest" },
               new Badge { Id = 16, ImagePath = "dependency.webp", Title = "Dependency Injection" },
                 new Badge { Id = 17, ImagePath = "events.png", Title = "Delegaty i zdarzenia" },
                 new Badge { Id = 18, ImagePath = "generyki.png", Title = "Generyki" }
          ); 
            modelBuilder.Entity<Challenge>().HasData(
                new Challenge { Id = 1, Name = "Pierwsze kroki w C#", Description = "Napisz prostą aplikację Hello World.", RewardDescription = "Odznaka: Początkujący", Points = 50, Type = "Coding" },
                new Challenge { Id = 2, Name = "Pętla for", Description = "Zaimplementuj program wypisujący liczby od 1 do 100.", RewardDescription = "Odznaka: Iteracje", Points = 70, Type = "Coding", BadgeId=3 },
                new Challenge { Id = 3, Name = "Warunki", Description = "Sprawdź, czy liczba jest parzysta lub nieparzysta.", RewardDescription = "Odznaka: Logika", Points = 80, Type = "Coding", BadgeId=4 },
                new Challenge { Id = 4, Name = "Listy", Description = "Stwórz listę i wypisz jej elementy.", RewardDescription = "Odznaka: Kolekcje", Points = 100, Type = "Coding", BadgeId=5 },
                new Challenge { Id = 5, Name = "Metody", Description = "Zaimplementuj metodę obliczającą silnię.", RewardDescription = "Odznaka: Rekurencja", Points = 120, Type = "Coding",BadgeId=7 },
                new Challenge { Id = 6, Name = "Klasy i obiekty", Description = "Stwórz klasę reprezentującą samochód.", RewardDescription = "Odznaka: OOP", Points = 150, Type = "Coding",BadgeId=1 },
                new Challenge { Id = 7, Name = "Dziedziczenie", Description = "Zaimplementuj klasę bazową i klasę dziedziczącą.", RewardDescription = "Odznaka: OOP+", Points = 200, Type = "Coding",BadgeId=8 },
                new Challenge { Id = 8, Name = "Interfejsy", Description = "Stwórz interfejs i zaimplementuj go w klasie.", RewardDescription = "Odznaka: Architekt", Points = 250, Type = "Coding",BadgeId=9 },
                new Challenge { Id = 9, Name = "Pliki", Description = "Napisz program zapisujący i odczytujący tekst z pliku.", RewardDescription = "Odznaka: IO", Points = 180, Type = "Coding",BadgeId=10 },
                new Challenge { Id = 10, Name = "LINQ podstawy", Description = "Użyj LINQ do wyszukania liczb parzystych w kolekcji.", RewardDescription = "Odznaka: LINQ", Points = 220, Type = "Coding",BadgeId=6 },
                new Challenge { Id = 11, Name = "Obsługa wyjątków", Description = "Napisz kod, który przechwyci i obsłuży wyjątek dzielenia przez zero.", RewardDescription = "Odznaka: TryCatch", Points = 160, Type = "Coding",BadgeId=11 },
                new Challenge { Id = 12, Name = "Asynchroniczność", Description = "Zaimplementuj metodę asynchroniczną korzystającą z async/await.", RewardDescription = "Odznaka: Async", Points = 300, Type = "Coding",BadgeId=12 },
                new Challenge { Id = 13, Name = "Baza danych", Description = "Połącz się z bazą danych i odczytaj listę rekordów.", RewardDescription = "Odznaka: DataAccess", Points = 350, Type = "Coding", BadgeId=13 },
                new Challenge { Id = 14, Name = "Testy jednostkowe", Description = "Napisz test jednostkowy dla prostej metody kalkulatora.", RewardDescription = "Odznaka: Tester", Points = 200, Type = "Coding",BadgeId=14 },
                new Challenge { Id = 15, Name = "API REST", Description = "Stwórz prosty kontroler API zwracający listę obiektów JSON.", RewardDescription = "Odznaka: WebDev", Points = 400, Type = "Coding",BadgeId=15 },
                new Challenge { Id = 16, Name = "Dependency Injection", Description = "Skonfiguruj wstrzykiwanie zależności w aplikacji ASP.NET Core.", RewardDescription = "Odznaka: Architektura", Points = 450, Type = "Coding",BadgeId=16 },
                new Challenge { Id = 17, Name = "Delegaty i zdarzenia", Description = "Utwórz delegata i zdarzenie, a następnie je wywołaj.", RewardDescription = "Odznaka: EventMaster", Points = 280, Type = "Coding", BadgeId=17 },
                new Challenge { Id = 18, Name = "Generics", Description = "Zaimplementuj klasę generyczną działającą dla różnych typów danych.", RewardDescription = "Odznaka: Generics", Points = 320, Type = "Coding", BadgeId=18 },
                new Challenge { Id = 19, Name = "Pierwsze kroki", Description = "Napisz prostą aplikację Hello World w dowolnym języku.", RewardDescription = "Odznaka: Początkujący", Points = 10, Type = "Coding",BadgeId=2 }
            );

            modelBuilder.Entity<WeeklyChallenge>().HasData(
                new WeeklyChallenge { Id = 1, ChallengeId = 1 },
                new WeeklyChallenge { Id = 2, ChallengeId = 2 },
                new WeeklyChallenge { Id = 3, ChallengeId = 3 }
            );

            modelBuilder.Entity<UserChallenge>()
                .HasOne(uc => uc.Challenge)
                .WithMany(c => c.UserChallenges)
                .HasForeignKey(uc => uc.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeeklyChallenge>()
                .HasOne(wc => wc.Challenge)
                .WithMany()
                .HasForeignKey(wc => wc.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);



            
        }
    }
}