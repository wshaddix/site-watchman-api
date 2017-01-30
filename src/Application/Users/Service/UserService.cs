using System.Linq;
using System.Threading.Tasks;
using SiteWatchman.Application.Shared.Services;
using SiteWatchman.Application.Users.Commands;
using SiteWatchman.Application.Users.Events;
using SiteWatchman.Application.Users.Exceptions;
using SiteWatchman.Application.Users.Factories;
using SiteWatchman.Application.Users.Models;
using SiteWatchman.Common.Application.Queries;
using SiteWatchman.Common.Infrastructure;
using SiteWatchman.Common.Persistance;
using SiteWatchman.Domain;

namespace SiteWatchman.Application.Users.Service
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IPasswordService _passwordService;

        public UserService(IDatabase database, IPasswordService passwordService) : base(database)
        {
            _passwordService = passwordService;
        }

        public async Task<UserModel> CreateAsync(UserCreateCommand cmd)
        {
            var result = await ExecuteAsync<UserCreateCommand, UserCreateCommandValidator, UserModel>(cmd, async () =>
            {
                // check to see if the user already exists by username
                var existingUser = Get<User>(u => u.Username.ToLower().Equals(cmd.Username.ToLower()));

                // if the user exists throw an exception b/c the usernames have to be unique
                if (null != existingUser) throw new UserExistsException($"A user with username {cmd.Username} already exists.");

                // create the user
                var user = new User(firstName: cmd.FirstName,
                                    email: cmd.Email,
                                    username: cmd.Username,
                                    hashedPassword: _passwordService.CreatePasswordHash(cmd.Password),
                                    createdByUserId: MessageContext.UserId);

                // save the user
                await Database.SaveAsync(user).ConfigureAwait(false);

                // convert the user to it's model
                var model = UserFactory.ConvertToModel(user);

                // publish the domain event
                Publish(new UserCreatedEvent(model, MessageContext));

                // return the user model
                return model;
            }).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(UserDeleteCommand cmd)
        {
            await ExecuteAsyncNoReturn<UserDeleteCommand, UserDeleteCommandValidator>(cmd, async () =>
            {
                // get the user by unique id
                var user = Get<User>(a => a.Id.Equals(cmd.Id));

                // if the user does not exist then there is nothing for us to do. The intent is satisfied
                if (null == user) return;

                // delete the user
                user.Delete(deletedByUserId: MessageContext.UserId);

                // save the deleted user (soft delete)
                await Database.SaveAsync(user).ConfigureAwait(false);

                // publish the domain event
                Publish(new UserDeletedEvent(UserFactory.ConvertToModel(user), MessageContext));
            }).ConfigureAwait(false);
        }

        public UserModel GetByEmail(string email)
        {
            // get the user by id
            var user = Database.Get<User>(a => a.Email.ToLower().Equals(email.ToLower()));

            // if the user doesn't exist return null
            if (null == user) return null;

            // publish the domain event
            Publish(new UserReadEvent(UserFactory.ConvertToModel(user), MessageContext));

            // return the user model
            return UserFactory.ConvertToModel(user);
        }

        public UserModel GetById(string id)
        {
            // get the user by id
            var user = Database.Get<User>(a => a.Id.Equals(id));

            // if the user doesn't exist return null
            if (null == user) return null;

            // publish the domain event
            Publish(new UserReadEvent(UserFactory.ConvertToModel(user), MessageContext));

            // return the user model
            return UserFactory.ConvertToModel(user);
        }

        public UserModel GetByUsername(string username)
        {
            // get the user by id
            var user = Database.Get<User>(a => a.Username.ToLower().Equals(username.ToLower()));

            // if the user doesn't exist return null
            if (null == user) return null;

            // publish the domain event
            Publish(new UserReadEvent(UserFactory.ConvertToModel(user), MessageContext));

            // return the user model
            return UserFactory.ConvertToModel(user);
        }

        public EntityListQueryResult<UserModel> List(string criteria, string sortBy, int page, int pageSize)
        {
            // fetch a list of users with a wildcard search
            criteria = criteria.ToLower().Trim();
            var usersListResult = Database.List<User>(where: u => string.IsNullOrWhiteSpace(criteria) ||
                                                                    u.FirstName.ToLower().Contains(criteria) ||
                                                                    u.Username.ToLower().Contains(criteria) ||
                                                                    u.Email.ToLower().Contains(criteria),
                                                      sortBy: sortBy,
                                                      page: page,
                                                      pageSize: pageSize);

            // publish the domain event
            Publish(new UserListedEvent(MessageContext));

            // convert to models and return
            return new EntityListQueryResult<UserModel>
            {
                TotalCount = usersListResult.TotalCount,
                TotalPages = usersListResult.TotalPages,
                DataList = usersListResult.DataList.Select(UserFactory.ConvertToModel).ToList()
            };
        }

        public async Task<UserModel> UpdateAsync(UserUpdateCommand cmd)
        {
            var result = await ExecuteAsync<UserUpdateCommand, UserUpdateCommandValidator, UserModel>(cmd, async () =>
            {
                // get the user by unique id
                var user = Get<User>(a => a.Id.Equals(cmd.Id));

                // if the user does not exist we need to throw an exception b/c the client believes it does
                if (null == user) throw new UserNotFoundException($"User with id {cmd.Id} does not exist.");

                // store the original user as it's model
                var originalUser = UserFactory.ConvertToModel(user);

                // update the user
                user.Update(firstName: cmd.FirstName,
                            email: cmd.Email,
                            username: cmd.Username,
                            updatedByUserId: MessageContext.UserId);

                // save the updated user
                await Database.SaveAsync(user).ConfigureAwait(false);

                // convert the user to it's model
                var model = UserFactory.ConvertToModel(user);

                // publish the domain event
                Publish(new UserUpdatedEvent(MessageContext)
                {
                    OriginalUser = originalUser,
                    UpdatedUser = model
                });

                // return the user model
                return model;
            }).ConfigureAwait(false);

            return result;
        }
    }
}