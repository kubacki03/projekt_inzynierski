namespace projekt_inzynierski.Server.Courses.Domain.Models
{
    public class FeaturedCourse
    {
        public int Id { get; set; } 
        public int CourseId { get; set; }  
        public Course Course { get; set; } 
    }
}
