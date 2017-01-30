using FluentValidation;

namespace SiteWatchman.Application.Applications.Commands
{
    public class ApplicationUpdateCommandValidator : AbstractValidator<ApplicationUpdateCommand>
    {
        public ApplicationUpdateCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty();
            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.ApiKey).NotEmpty();
        }
    }
}