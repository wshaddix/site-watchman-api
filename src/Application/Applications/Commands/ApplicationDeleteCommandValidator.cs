using FluentValidation;

namespace SiteWatchman.Application.Applications.Commands
{
    public class ApplicationDeleteCommandValidator : AbstractValidator<ApplicationDeleteCommand>
    {
        public ApplicationDeleteCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}