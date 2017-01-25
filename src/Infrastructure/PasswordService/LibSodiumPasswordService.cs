using SiteWatchman.Common.Infrastructure;
using Sodium;

namespace SiteWatchman.Infrastructure.PasswordService
{
    public class LibSodiumPasswordService : IPasswordService
    {
        public string CreatePasswordHash(string password)
        {
            return PasswordHash.ScryptHashString(password);
        }

        public bool ValidatePassword(string password, string passwordHash)
        {
            return PasswordHash.ScryptHashStringVerify(passwordHash, password);
        }
    }
}