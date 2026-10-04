namespace projekt_inzynierski.Server.Content.Domain.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int SubjectId { get; set; } 
        public bool Done { get; set; }
        public int Attempts { get; set; } 
        public List<Answer> Answers { get; set; } = new List<Answer>(); 
        public Subject Subject { get; set; } 
    }
}
