namespace SiteWatchman.Common.Infrastructure
{
    public interface IPasswordService
    {
        string CreatePasswordHash(string password);
        bool ValidatePassword(string password, string passwordHash);
    }
}