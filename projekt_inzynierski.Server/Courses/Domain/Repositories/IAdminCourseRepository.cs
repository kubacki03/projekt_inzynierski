namespace projekt_inzynierski.Server.Courses.Domain.Repositories
{
    public interface IAdminCourseRepository
    {
         Task<int> GetCourseCount();
    }
}
