using FluentValidation;

namespace SiteWatchman.Application.Users.Commands
{
    public class UserDeleteCommandValidator : AbstractValidator<UserDeleteCommand>
    {
        public UserDeleteCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}