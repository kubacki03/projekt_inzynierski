namespace projekt_inzynierski.Server.Courses.Application.DTOs
{
    public class UserCourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public DateTime? CreatedDate { get; set; } = default(DateTime?);
        public float Progress { get; set; }
        public string Image { get; set; }
    }
}
