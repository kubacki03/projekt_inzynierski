using System.ComponentModel.DataAnnotations;
using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;

namespace projekt_inzynierski.Server.Admin.Application.DTOs
{
    public class CourseMaterialDto
    {
        [Required]
        public string Type { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Question { get; set; }
        public List<string>? Answers { get; set; }
        public int? CorrectAnswer { get; set; }
        public int CourseId { get; set; }
        public string SubjectTitle {  get; set; }
    }
}
