using SiteWatchman.Application.Users.Models;
using SiteWatchman.Domain.Users;

namespace SiteWatchman.Application.Users.Factories
{
    public class UserFactory
    {
        internal static UserModel ConvertToModel(User user)
        {
            if (user == null) return null;

            return new UserModel
            {
                Username = user.Username
            };
        }
    }
}