using System.Threading.Tasks;
using SiteWatchman.Application.Applications.Commands.CreateApplication;
using SiteWatchman.Application.Platform.Models;
using SiteWatchman.Application.Users.Commands.CreateUser;
using SiteWatchman.Common.Application;

namespace SiteWatchman.Application.Platform.Commands.CreatePlatform
{
    public sealed class CreatePlatformCommand : MessageBase<CreatePlatformCommand, CreatePlatformValidator, PlatformModel>,
                                                ICreatePlatformCommand
    {
        private readonly ICreateApplicationCommand _createApplicationCommand;
        private readonly ICreateUserCommand _createUserCommand;
        internal string PlatformAdminPassword;
        internal string PlatformAdminUsername;
        internal string ApplicationApiKey;
        internal string ApplicationName;

        public CreatePlatformCommand(ICreateApplicationCommand createApplicationCommand, ICreateUserCommand createUserCommand)
        {
            _createApplicationCommand = createApplicationCommand;
            _createUserCommand = createUserCommand;
        }

        public async Task<PlatformModel> ExecuteAsync(string platformAdminUsername,
                                     string platformAdminPassword,
                                     string applicationName,
                                     string applicationApiKey)
        {
            // clean, initialize and assign fields
            PlatformAdminUsername   = platformAdminUsername.Trim();
            PlatformAdminPassword   = platformAdminPassword.Trim();
            ApplicationName         = applicationName.Trim();
            ApplicationApiKey       = applicationApiKey.Trim();

            // execute the message pipeline and capture the results
            var result = await base.ExecuteAsync(this, async () =>
            {
                // create the application
                var app = await _createApplicationCommand.ExecuteAsync(
                                                                applicationName: ApplicationName,
                                                                apiKey: ApplicationApiKey);

                // create the user
                var user = await _createUserCommand.ExecuteAsync(
                                                                username: PlatformAdminUsername,
                                                                password: PlatformAdminPassword);

                return new PlatformModel(app, user);
            });

             // return the application model
            return result;
        }
    }
}