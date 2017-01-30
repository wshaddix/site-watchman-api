using System.Linq;
using System.Threading.Tasks;
using SiteWatchman.Application.Applications.Commands;
using SiteWatchman.Application.Applications.Events;
using SiteWatchman.Application.Applications.Exceptions;
using SiteWatchman.Application.Applications.Factories;
using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Shared.Services;
using SiteWatchman.Common.Application.Queries;
using SiteWatchman.Common.Persistance;

namespace SiteWatchman.Application.Applications.Service
{
    public class ApplicationService : ServiceBase, IApplicationService
    {
        public ApplicationService(IDatabase database) : base(database)
        {
        }

        public async Task<ApplicationModel> CreateAsync(ApplicationCreateCommand cmd)
        {
            var result = await ExecuteAsync<ApplicationCreateCommand, ApplicationCreateCommandValidator, ApplicationModel>(cmd, async () =>
            {
                // check to see if the application already exists by name
                var existingApp = Get<Domain.Application>(a => a.Name.ToLower().Equals(cmd.Name.ToLower()));

                // if the application exists just return it
                if (null != existingApp) return ApplicationFactory.ConvertToModel(existingApp);

                // create the application
                var app = new Domain.Application(name: cmd.Name,
                                                 apiKey: cmd.ApiKey,
                                                 createdByUserId: MessageContext.UserId);

                // save the application
                await Database.SaveAsync(app).ConfigureAwait(false);

                // convert the application to it's model
                var model = ApplicationFactory.ConvertToModel(app);

                // publish the domain event
                Publish(new ApplicationCreatedEvent(model, MessageContext));

                // return the application model
                return model;
            }).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(ApplicationDeleteCommand cmd)
        {
            await ExecuteAsyncNoReturn<ApplicationDeleteCommand, ApplicationDeleteCommandValidator>(cmd, async () =>
            {
                // get the application by unique id
                var application = Get<Domain.Application>(a => a.Id.Equals(cmd.Id));

                // if the application does not exist then there is nothing for us to do. The intent is satisfied
                if (null == application) return;

                // delete the application
                application.Delete(deletedByUserId: MessageContext.UserId);

                // save the deleted application (soft delete)
                await Database.SaveAsync(application).ConfigureAwait(false);

                // publish the domain event
                Publish(new ApplicationDeletedEvent(ApplicationFactory.ConvertToModel(application), MessageContext));
            }).ConfigureAwait(false);
        }

        public ApplicationModel GetById(string id)
        {
            // get the application by id
            var application = Database.Get<Domain.Application>(a => a.Id.Equals(id));

            // if the application doesn't exist return null
            if (null == application) return null;

            // publish the domain event
            Publish(new ApplicationReadEvent(ApplicationFactory.ConvertToModel(application), MessageContext));

            // return the application model
            return ApplicationFactory.ConvertToModel(application);
        }

        public ApplicationModel GetByName(string name)
        {
            // get the application by id
            var application = Database.Get<Domain.Application>(a => a.Name.ToLower().Equals(name.ToLower()));

            // if the application doesn't exist return null
            if (null == application) return null;

            // publish the domain event
            Publish(new ApplicationReadEvent(ApplicationFactory.ConvertToModel(application), MessageContext));

            // return the application model
            return ApplicationFactory.ConvertToModel(application);
        }

        public EntityListQueryResult<ApplicationModel> List(string criteria, string sortBy, int page, int pageSize)
        {
            var applicationsListResult = Database.List<Domain.Application>(
                                                                    where: m => string.IsNullOrWhiteSpace(criteria) ||
                                                                                m.Name.ToLower().Contains(criteria) ||
                                                                                m.ApiKey.Contains(criteria),
                                                                    sortBy: sortBy,
                                                                    page: page,
                                                                    pageSize: pageSize);

            // publish the domain event
            Publish(new ApplicationListedEvent(MessageContext));

            // convert to models and return
            return new EntityListQueryResult<ApplicationModel>
            {
                TotalCount = applicationsListResult.TotalCount,
                TotalPages = applicationsListResult.TotalPages,
                DataList = applicationsListResult.DataList.Select(ApplicationFactory.ConvertToModel).ToList()
            };
        }

        public async Task<ApplicationModel> UpdateAsync(ApplicationUpdateCommand cmd)
        {
            var result = await ExecuteAsync<ApplicationUpdateCommand, ApplicationUpdateCommandValidator, ApplicationModel>(cmd, async () =>
            {
                // get the application by unique id
                var application = Get<Domain.Application>(a => a.Id.Equals(cmd.Id));

                // if the application does not exist we need to throw an exception b/c the client believes it does
                if (null == application) throw new ApplicationNotFoundException($"Application with id {cmd.Id} does not exist.");

                // store the original application as it's model
                var originalApplication = ApplicationFactory.ConvertToModel(application);

                // update the application
                application.Update(name: cmd.Name, updatedByUserId: MessageContext.UserId, resetApiKey: cmd.ResetApiKey);

                // save the updated application
                await Database.SaveAsync(application).ConfigureAwait(false);

                // convert the application to it's model
                var model = ApplicationFactory.ConvertToModel(application);

                // publish the domain event
                Publish(new ApplicationUpdatedEvent(MessageContext)
                {
                    OriginalApplication = originalApplication,
                    UpdatedApplication = model
                });

                // return the application model
                return model;
            }).ConfigureAwait(false);

            return result;
        }
    }
}