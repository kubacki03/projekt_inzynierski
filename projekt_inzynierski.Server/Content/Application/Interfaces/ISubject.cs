using System.Threading.Tasks;
using System.Xml.Serialization;
using projekt_inzynierski.Server.AIHelper.Application.DTOs;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Content.Application.Interfaces
{
    public interface ISubject
    {
        Task<List<string>> GenerateSubjects(string courseName, string courseDescription, int courseId, User user);
        Task<List<SubjectDto>> GetSubjects(int courseId, string userId);
        Task<List<Theory>> GetTheory(int subjectId);
        Task<ContentModel> GenerateContent(string userId, int subjectId, User user);
        Task<List<ExerciseDto>> GetUserExercise(int subjectId, string userId);
        Task<List<Quiz>> GetUserQuiz(int subjectId, string userId);
        Task<Exercise> GetExerciseById(int exerciseId);
        Task<float> GetUserMistakesCount(string userId, int subjectId);
        Task<Theory> GenerateTheoryFromPdf(int subjectId, string courseName, string courseDescription, User user, string pdfPath);
    }
}
