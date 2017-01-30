using System;
using System.Configuration;
using System.IO;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using SiteWatchman.Application.Applications.Commands;
using SiteWatchman.Application.Applications.Service;
using SiteWatchman.Application.Shared.Messages;
using SiteWatchman.Application.Users.Commands;
using SiteWatchman.Application.Users.Service;
using SiteWatchman.Common.Infrastructure;
using SiteWatchman.Common.Persistance;
using SiteWatchman.Infrastructure.PasswordService;
using SiteWatchman.Persistance;

namespace SiteWatchman.Api
{
    public class SiteWatchmanBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            // We are using libsodium for password hashing which includes unmanaged code so we have to include our bin folder in our PATH so that when
            // Sodium.dll(managed code) tries to load libsodium-64.dll(unmanaged code) it will be able to find it http://stackoverflow.com/a/37526795
            var path = Environment.GetEnvironmentVariable("PATH");
            var binDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");
            Environment.SetEnvironmentVariable("PATH", path + ";" + binDir);

            // create a documentdb client
            var database = new DocumentDbDatabase(serviceEndpoint: ConfigurationManager.AppSettings["DocumentDb.Uri"],
                                                    authKey: ConfigurationManager.AppSettings["DocumentDb.AuthKey"],
                                                    databaseId: ConfigurationManager.AppSettings["DocumentDb.DatabaseId"],
                                                    collectionId: ConfigurationManager.AppSettings["DocumentDb.CollectionId"]);

            // register a password service
            container.Register<IPasswordService, LibSodiumPasswordService>().AsSingleton();

            // register a database
            container.Register<IDatabase, DocumentDbDatabase>(database);

            // setup the message context that will be used for platform initialization
            var messageContext = new MessageContext
            {
                Username = "SiteWatchmanBootstrapper",
                ApplicationName = "SiteWatchman.Api",
                UserId = "0000",
                ApplicationId = "0000",
                Environment = ConfigurationManager.AppSettings["Environment"],
                HostName = Environment.MachineName,
                Region = ConfigurationManager.AppSettings["Region"],
                TenantName = "Platform",
                UserSessionId = "0000",
                Version = ConfigurationManager.AppSettings["Version"]
            };

            // now that the application has started, we need to ensure the platform admin user exists
            var userService = container.Resolve<IUserService>();
            userService.MessageContext = messageContext;

            userService.CreateAsync(new UserCreateCommand
            {
                Username = ConfigurationManager.AppSettings["PlatformAdmin.Username"],
                Password = ConfigurationManager.AppSettings["PlatformAdmin.Password"],
                FirstName = ConfigurationManager.AppSettings["PlatformAdmin.FirstName"],
                Email = ConfigurationManager.AppSettings["PlatformAdmin.Email"]
            }).ConfigureAwait(false);

            // ensure the the default application (postman) exists
            var applicationService = container.Resolve<IApplicationService>();
            applicationService.MessageContext = messageContext;

            applicationService.CreateAsync(new ApplicationCreateCommand
            {
                ApiKey = ConfigurationManager.AppSettings["Application.ApiKey"],
                Name = ConfigurationManager.AppSettings["Application.Name"]
            }).ConfigureAwait(false);
        }
    }
}