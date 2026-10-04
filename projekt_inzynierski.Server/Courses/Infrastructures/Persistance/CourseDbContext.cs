using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Courses.Infrastructures.Persistance
{
    public class CourseDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<FeaturedCourse> FeaturedCourses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }

        public CourseDbContext(DbContextOptions<CourseDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeaturedCourse>()
                .HasOne(fc => fc.Course)
                .WithOne(c => c.FeaturedCourse)
                .HasForeignKey<FeaturedCourse>(fc => fc.CourseId);


            modelBuilder.Entity<Course>()
              .HasIndex(x => x.Id);

            modelBuilder.Entity<UserCourse>()
              .HasIndex(x => x.UserId);


            modelBuilder.Entity<UserCourse>()
                .Property(e => e.EvaluationResult)
                .HasConversion<string>();

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,

                    Title = "C# dla początkujących",
                    Description = "Podstawy programowania w języku C#: zmienne, pętle, klasy i obiekty.",
                    Language = "Polski",
                    ImageURL = "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056",
                    IsPublic = true,
                    Level = "Beginner"
                },
                new Course
                {
                    Id = 2,

                    Title = "Python – pierwsze kroki",
                    Description = "Wprowadzenie do Pythona. Naucz się pisać skrypty, pracować z listami i funkcjami.",
                    Language = "Polski",
                    ImageURL = "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056",
                    IsPublic = true,
                    Level = "Beginner"
                },
                new Course
                {
                    Id = 3,

                    Title = "Java – programowanie obiektowe",
                    Description = "Kurs skupiający się na fundamentach OOP w Javie: dziedziczenie, polimorfizm, interfejsy.",
                    Language = "Polski",
                    ImageURL = "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056",
                    IsPublic = true,
                    Level = "Intermediate"
                },
                new Course
                {
                    Id = 4,

                    Title = "ASP.NET Core – tworzenie API",
                    Description = "Budowanie nowoczesnych API z użyciem ASP.NET Core i Entity Framework.",
                    Language = "Polski",
                    ImageURL = "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056",
                    IsPublic = true,
                    Level = "Intermediate"
                },
                new Course
                {
                    Id = 5,

                    Title = "JavaScript – od podstaw do React",
                    Description = "Naucz się JavaScript, DOM i podstaw Reacta, aby tworzyć dynamiczne aplikacje webowe.",
                    Language = "Polski",
                    ImageURL = "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056",
                    IsPublic = true,
                    Level = "Beginner"
                }
            ); 

            modelBuilder.Entity<FeaturedCourse>().HasData(
                new FeaturedCourse { Id = 1, CourseId = 1 },
                new FeaturedCourse { Id = 2, CourseId = 3 }
            );
        }
    }
}
