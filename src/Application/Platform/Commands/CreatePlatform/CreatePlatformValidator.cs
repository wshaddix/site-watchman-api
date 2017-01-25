using FluentValidation;

namespace SiteWatchman.Application.Platform.Commands.CreatePlatform
{
    public class CreatePlatformValidator : AbstractValidator<CreatePlatformCommand>
    {
        public CreatePlatformValidator()
        {
            RuleFor(cmd => cmd.PlatformAdminUsername).NotEmpty();
            RuleFor(cmd => cmd.PlatformAdminPassword).NotEmpty();
            RuleFor(cmd => cmd.ApplicationName).NotEmpty();
        }
    }
}