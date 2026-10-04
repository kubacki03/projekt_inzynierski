namespace projekt_inzynierski.Server.Courses.Application.Interfaces
{
    public interface IAdminCourseService
    {
        Task<int> GetCourseCount();
        Task<int> CreateCourse(string title, string description, bool isPublic, string language, string level);
    }
}
