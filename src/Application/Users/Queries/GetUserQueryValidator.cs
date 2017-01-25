using FluentValidation;

namespace SiteWatchman.Application.Users.Queries
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(q => q.Where).NotNull();
        }
    }
}