using System;
using System.Linq.Expressions;
using SiteWatchman.Application.Applications.Models;

namespace SiteWatchman.Application.Applications.Queries.GetApplicationQuery
{
    public interface IGetApplicationQuery
    {
        ApplicationModel Execute(Expression<Func<Domain.Applications.Application, bool>> where);
    }
}