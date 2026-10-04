namespace projekt_inzynierski.Server.Users.Application.DTOs
{
    public class UserRegisterDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string nickname { get; set; }
        public DateTime birthDate { get; set; }
        public string educationLevel { get; set; }
        public string experience { get; set; }
        public string gender { get; set; }
    }
}
