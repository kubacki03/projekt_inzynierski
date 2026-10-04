namespace projekt_inzynierski.Server.Content.Application.DTOs
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public int SubjectId { get; set; }
        public bool IsDone { get; set; }
    }

}
