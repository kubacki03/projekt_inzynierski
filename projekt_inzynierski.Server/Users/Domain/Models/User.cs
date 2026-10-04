using projekt_inzynierski.Server.PremiumStore.Domain.Models;

namespace projekt_inzynierski.Server.Users.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public DateTime AccountPremiumDateEnd { get; set; } 
        public Guid PublicId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string Nickname { get; set; }
        public DateTime BirthDate { get; set; }
        public string EducationLevel { get; set; }
        public string Experience { get; set; }
        public string Gender { get; set; }
        public long Points { get; set; } = 0;
        public long GoldenPoints { get; set; } = 0;
        public int SelectedAvatarId { get; set; } = 8;
        public bool Banned { get; set; } = false;

        public ICollection<Notification> Notifications { get; set; }
        public User(Guid id, string email, string passwordHash)
        {
            PublicId = id;
            Email = email;
            PasswordHash = passwordHash;
        }

        public User(int id, Guid publicId, string email, string passwordHash)
        {
            Id = id;
            PublicId = publicId;
            Email = email;
            PasswordHash = passwordHash;
        }
        public User(Guid publicId, string email, string passwordHash, string firstName, string nickname, DateTime birthDate, string educationLevel, string experience, string gender)
        {
            PublicId = publicId;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            Nickname = nickname;
            BirthDate = birthDate;
            EducationLevel = educationLevel;
            Experience = experience;
            Gender = gender;
        }

        public User() { }

    }
}