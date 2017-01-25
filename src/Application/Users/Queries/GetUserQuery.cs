using System;
using System.Linq.Expressions;
using SiteWatchman.Application.Users.Factories;
using SiteWatchman.Application.Users.Models;
using SiteWatchman.Common.Application;
using SiteWatchman.Common.Persistance;
using SiteWatchman.Domain.Users;

namespace SiteWatchman.Application.Users.Queries
{
    public class GetUserQuery : MessageBase<GetUserQuery, GetUserQueryValidator, UserModel>, IGetUserQuery
    {
        private readonly IDatabase _db;
        internal Expression<Func<User, bool>> Where;

        public GetUserQuery(IDatabase db)
        {
            _db = db;
        }
        public UserModel Execute(Expression<Func<User, bool>> where)
        {
            // clean, initialize and assign fields
            Where = where;

            // execute the message pipeline and capture the results
            return base.Execute(this, () =>
            {
                // fetch the user based on the criteria
                var user = _db.Get(Where);

                // convert the user to a user model
                return UserFactory.ConvertToModel(user);
            });
        }
    }
}