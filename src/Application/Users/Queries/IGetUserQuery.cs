using System;
using System.Linq.Expressions;
using SiteWatchman.Application.Users.Models;
using SiteWatchman.Domain.Users;

namespace SiteWatchman.Application.Users.Queries
{
    public interface IGetUserQuery
    {
        UserModel Execute(Expression<Func<User, bool>> where);
    }
}