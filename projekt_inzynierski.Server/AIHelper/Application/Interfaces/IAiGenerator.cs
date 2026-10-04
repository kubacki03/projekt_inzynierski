using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.AIHelper.Application.DTOs;
using projekt_inzynierski.Server.AIHelper.Infrastructures.Services;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.AIHelper.Application.Interfaces
{
    public interface IAiGenerator
    {
        Task<AdaptiveCourseWithSubjectsAndContent> GenerateSubjectFromPdf(string courseName, string courseDescription, User user, string pdfPath);
        Task<List<string>> GenerateSubjects(string courseName, string courseDescription, User user);
        Task<ContentModel> GenerateContent(string subject, string course, string level, User user, string evaluationResult);
        Task<NotificationDto> GenerateSuggestAsync(List<string> problems, string mainCourseTitle);
        Task<Theory> GenerateTheoryFromPdf(string courseName, string courseDescription, User user, string pdfPath);

        Task<AdaptiveCourseWithSubjectsAndContent> GeneratePrivateSubjectsAndCourse(AdaptiveCourseDto courseDto);
    }
}
