namespace projekt_inzynierski.Server.Content.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsTrue { get; set; }
        public Quiz Quiz { get; set; }
    }
}
