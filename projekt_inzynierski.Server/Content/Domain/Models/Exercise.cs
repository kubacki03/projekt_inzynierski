namespace projekt_inzynierski.Server.Content.Domain.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public int SubjectId { get; set; }
        public bool Done { get; set; }
        public int Attempts { get; set; }
        public Subject Subject { get; set; }
        public ICollection<UserExerciseFeedback> Feedbacks { get; set; } = new List<UserExerciseFeedback>();
    }
}
