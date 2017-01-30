using SiteWatchman.Application.Shared.Models;
using SiteWatchman.Application.Users.Models;
using SiteWatchman.Domain;

namespace SiteWatchman.Application.Users.Factories
{
    internal class UserFactory : ModelFactoryBase
    {
        internal static UserModel ConvertToModel(User user)
        {
            if (null == user) return null;

            var model = MapBaseProperties<UserModel>(user);

            model.Username = user.Username;
            model.FirstName = user.FirstName;
            model.Email = user.Email;

            return model;
        }
    }
}