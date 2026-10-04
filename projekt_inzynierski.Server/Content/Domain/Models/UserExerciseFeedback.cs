namespace projekt_inzynierski.Server.Content.Domain.Models
{
    public class UserExerciseFeedback
    {
        public int Id { get; set; } 
        public string Feedback { get; set; }
        public int ExerciesId { get; set; }
        public Exercise Exercise { get; set; }
       
    }
}
