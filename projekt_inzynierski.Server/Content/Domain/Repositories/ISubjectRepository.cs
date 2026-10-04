using projekt_inzynierski.Server.AIHelper.Application.DTOs;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Domain.Models;

namespace projekt_inzynierski.Server.Content.Domain.Repositories
{
    public interface ISubjectRepository
    {
        Task AddSubjects(List<string> subjects, int courseId, string userId); 
        List<Subject> GetSubjects(int subjectId); 
        Task<List<Theory>> GetTheories(int subjectId); 
        Task<Subject> GetSubjectIdByTitleAndCourseId(string title, int courseId);
        Task<Subject> GetSubjectByQuizId(int quyizId);
        Task<List<Subject>> GetExercises(int subjectId); 
        Task<Subject> GetSubject(int subjectId); 
        Task AddContent(ContentModel contentModel, int subjectId); 
        Task<List<Quiz>> GetUserQuiz(int subjectId, string userId); 
        Task<Exercise> GetExerciseById(int exerciseId); 
        Task<List<Exercise>> GetUserExercise(int subjectId);
        Task IncreaseUserSubjectProgress(string userId, int subjectId); 
        Task<bool> DoesContentExistInSubject(int subjectId); 
        Task<float> GetUserMistakesCount(string userId, int subjectId);
        Task<List<string>> GetUserProblemsInCourse(int courseId, string userId); 
        Task<string> GetEvaluatorResultInCourseBySubjectId(string userId, int subjectId);
        Task AddTheoryToSubject(int subjectId, Theory t); 
        Task<float> GetProgressInSubject(int subjectId, string userId); 
        Task<List<SubjectDto>> GetProgressInList(List<Subject> list, string userId); 
    }
}
