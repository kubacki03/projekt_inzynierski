namespace projekt_inzynierski.Server.Content.Domain.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public int CourseId { get; set; } 
        public string UserId { get; set; } 
        public int Progress { get; set; } = 0;
        public int TasksToDo { get; set; } = 0;
        public ICollection<Quiz> Quizzes { get; set; }
        public ICollection<Theory> Theories { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
   
    }

}
