using FluentValidation;

namespace SiteWatchman.Application.Users.Commands
{
    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        public UserUpdateCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.Username).NotEmpty();
        }
    }
}