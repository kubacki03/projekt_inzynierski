namespace projekt_inzynierski.Server.Content.Domain.Models
{
    public class Theory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; } 
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
 