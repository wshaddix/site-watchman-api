using SiteWatchman.Common.Domain;

namespace SiteWatchman.Domain.Users
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }

        public User(string username, string hashedPassword)
        {
            Username = username;
            HashedPassword = hashedPassword;
        }
    }
}