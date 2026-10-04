using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace projekt_inzynierski.Server.Courses.Domain.Models
{

    public class Course
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        [AllowNull]
        public string? ImageURL { get; set; } = "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056";
        public bool IsPublic { get; set; }
        public string Level { get; set; }
        public FeaturedCourse FeaturedCourse { get; set; } 
        public List<UserCourse> UserCourses { get; set; }
    }
}
