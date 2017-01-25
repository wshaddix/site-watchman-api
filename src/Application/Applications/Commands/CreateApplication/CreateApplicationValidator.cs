using FluentValidation;

namespace SiteWatchman.Application.Applications.Commands.CreateApplication
{
    public class CreateApplicationValidator : AbstractValidator<CreateApplicationCommand>
    {
        public CreateApplicationValidator()
        {
            RuleFor(cmd => cmd.ApplicationName).NotEmpty();
            RuleFor(cmd => cmd.ApiKey).NotEmpty();
        }
    }
}