using System;
using System.Configuration;
using System.IO;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using SiteWatchman.Application.Applications.Commands.CreateApplication;
using SiteWatchman.Application.Platform.Commands.CreatePlatform;
using SiteWatchman.Application.Users.Commands.CreateUser;
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

            // We are using libsodium for password hashing which includes unmanaged code so we have
            // to include our bin folder in our PATH so that when Sodium.dll(managed code) tries to
            // load libsodium-64.dll(unmanaged code) it will be able to find it
            // http://stackoverflow.com/a/37526795
            var path = Environment.GetEnvironmentVariable("PATH");
            var binDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");
            Environment.SetEnvironmentVariable("PATH", path + ";" + binDir);

            // create a documentdb client
            var database = new DocumentDbDatabase(  serviceEndpoint: ConfigurationManager.AppSettings["DocumentDb.Uri"],
                                                    authKey:         ConfigurationManager.AppSettings["DocumentDb.AuthKey"],
                                                    databaseId:      ConfigurationManager.AppSettings["DocumentDb.DatabaseId"],
                                                    collectionId:    ConfigurationManager.AppSettings["DocumentDb.CollectionId"]);

            // register a password service
            container.Register<IPasswordService, LibSodiumPasswordService>().AsSingleton();

            // register a database
            container.Register<IDatabase, DocumentDbDatabase>(database);

            // now that the application has started, we need to initialize the platform
            var cmd = new CreatePlatformCommand(container.Resolve<ICreateApplicationCommand>(),
                                                container.Resolve<ICreateUserCommand>());

            var task = cmd.ExecuteAsync(platformAdminUsername: ConfigurationManager.AppSettings["PlatformAdmin.Username"],
                        platformAdminPassword: ConfigurationManager.AppSettings["PlatformAdmin.Password"],
                        applicationName:       ConfigurationManager.AppSettings["Application.Name"],
                        applicationApiKey:     ConfigurationManager.AppSettings["Application.ApiKey"]);

            task.ConfigureAwait(false);
            task.Wait();

        }
    }
}