using System.Threading.Tasks;
using SiteWatchman.Application.Users.Exceptions;
using SiteWatchman.Application.Users.Factories;
using SiteWatchman.Application.Users.Models;
using SiteWatchman.Application.Users.Queries;
using SiteWatchman.Common.Application;
using SiteWatchman.Common.Infrastructure;
using SiteWatchman.Common.Persistance;
using SiteWatchman.Domain.Users;

namespace SiteWatchman.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : MessageBase<CreateUserCommand, CreateUserValidator, UserModel>, ICreateUserCommand
    {
        private readonly IDatabase _db;
        private readonly IGetUserQuery _getUserQuery;
        private readonly IPasswordService _passwordService;
        internal string Username;
        internal string Password;

        public CreateUserCommand(IDatabase db, IGetUserQuery getUserQuery, IPasswordService passwordService)
        {
            _db = db;
            _getUserQuery = getUserQuery;
            _passwordService = passwordService;
        }

        public async Task<UserModel> ExecuteAsync(string username, string password)
        {
            // clean, initialize and assign fields
            Username = username.Trim().ToLower();
            Password = password.Trim();

            // execute the command pipeline and capture the results
            var result = await base.ExecuteAsync(this, async () =>
            {
                // check to see if a user already exists by name
                var existingUser = _getUserQuery.Execute(u => u.Username.ToLower().Equals(Username));

                // if the user exists return null
                if (null != existingUser) return null;

                // hash the password
                var passwordHash = _passwordService.CreatePasswordHash(Password);

                // create the user
                var user = new User(username: Username, hashedPassword: passwordHash);

                // save the user
                await _db.SaveAsync(user);

                // return the user model
                return UserFactory.ConvertToModel(user);
            });

            // return the application model
            return result;
        }
    }
}