using FluentValidation;

namespace SiteWatchman.Application.Applications.Queries.GetApplicationQuery
{
    public class GetApplicationQueryValidator : AbstractValidator<GetApplicationQuery>
    {
        public GetApplicationQueryValidator()
        {
            RuleFor(q => q.Where).NotNull();
        }
    }
}