using System;
using System.Linq.Expressions;
using SiteWatchman.Application.Applications.Factories;
using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Common.Application;
using SiteWatchman.Common.Persistance;

namespace SiteWatchman.Application.Applications.Queries.GetApplicationQuery
{
    public class GetApplicationQuery : MessageBase<GetApplicationQuery, GetApplicationQueryValidator, ApplicationModel>, IGetApplicationQuery
    {
        private readonly IDatabase _db;
        internal Expression<Func<Domain.Applications.Application, bool>> Where;

        public GetApplicationQuery(IDatabase db)
        {
            _db = db;
        }

        public ApplicationModel Execute(Expression<Func<Domain.Applications.Application, bool>> where)
        {
            // clean, initialize and assign fields
            Where = where;

            // execute the message pipeline and capture the results
            return base.Execute(this, () =>
            {
                // fetch the application based on the criteria
                var application = _db.Get(Where);

                // convert the application to an application model
                return ApplicationFactory.ConvertToModel(application);
            });
        }
    }
}