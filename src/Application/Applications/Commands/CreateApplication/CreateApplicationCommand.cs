using System;
using System.Threading.Tasks;
using SiteWatchman.Application.Applications.Factories;
using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Applications.Queries.GetApplicationQuery;
using SiteWatchman.Common.Application;
using SiteWatchman.Common.Persistance;

namespace SiteWatchman.Application.Applications.Commands.CreateApplication
{
    public class CreateApplicationCommand : MessageBase<CreateApplicationCommand, CreateApplicationValidator, ApplicationModel>,
                                                ICreateApplicationCommand
    {
        private readonly IDatabase _db;
        private readonly IGetApplicationQuery _getApplicationQuery;
        internal string ApplicationName;
        internal string ApiKey;

        public CreateApplicationCommand(IDatabase db, IGetApplicationQuery getApplicationQuery)
        {
            _db = db;
            _getApplicationQuery = getApplicationQuery;
        }

        public async Task<ApplicationModel> ExecuteAsync(string applicationName, string apiKey = null)
        {
            // clean, initialize and assign fields
            ApplicationName = applicationName.Trim().ToLower();
            ApiKey = apiKey ?? Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

            // execute the command pipeline and capture the results
            var result = await base.ExecuteAsync(this, async () =>
            {
                // check to see if the application already exists by name
                var existingApp = _getApplicationQuery.Execute(a => a.Name.ToLower().Equals(ApplicationName) && a.ApiKey.Equals(ApiKey));

                // if the application exists just return it
                if (null != existingApp) return existingApp;

                // create the application
                var app = new Domain.Applications.Application(name: ApplicationName, apiKey: ApiKey);

                // save the application
                await _db.SaveAsync(app);

                // return the application model
                return ApplicationFactory.ConvertToModel(app);
            });

            // return the results
            return result;
        }
    }
}