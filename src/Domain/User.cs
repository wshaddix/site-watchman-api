using System;
using SiteWatchman.Common.Domain;

namespace SiteWatchman.Domain
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string HashedPassword { get; set; }
        public string Username { get; set; }

        public User(string firstName, string email, string username, string hashedPassword, string createdByUserId)
        {
            Username = username;
            HashedPassword = hashedPassword;
            FirstName = firstName;
            CreatedByUserId = createdByUserId;
            Email = email;
        }

        public void Delete(string deletedByUserId)
        {
            IsDeleted = true;
            DeletedByUserId = deletedByUserId;
            DeletedOnUtc = DateTime.UtcNow;
        }

        public void Update(string firstName, string email, string username, string updatedByUserId)
        {
            Username = username;
            FirstName = firstName;
            UpdatedByUserId = updatedByUserId;
            Email = email;
            UpdatedOnUtc = DateTime.UtcNow;
        }
    }
}