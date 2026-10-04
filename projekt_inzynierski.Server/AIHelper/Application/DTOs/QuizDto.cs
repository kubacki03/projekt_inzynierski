namespace projekt_inzynierski.Server.AIHelper.Application.DTOs
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public bool IsCompleted { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}
