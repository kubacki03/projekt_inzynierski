namespace projekt_inzynierski.Server.Users.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Nickname { get; set; }
        public DateTime BirthDate { get; set; }
        public string EducationLevel { get; set; }
        public string Experience { get; set; }
        public string Gender { get; set; }
        public long Points { get; set; }
        public long GoldenPoints { get; set; }
        public string SelectedAvatar { get; set; }
        public DateTime Date { get; set; }
    }
}
