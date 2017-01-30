using FluentValidation;

namespace SiteWatchman.Application.Applications.Commands
{
    public class ApplicationCreateCommandValidator : AbstractValidator<ApplicationCreateCommand>
    {
        public ApplicationCreateCommandValidator()
        {
            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.ApiKey).NotEmpty();
        }
    }
}