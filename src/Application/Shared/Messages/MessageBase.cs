using FluentValidation;

namespace SiteWatchman.Application.Shared.Messages
{
    public abstract class MessageBase<TMessage, TValidator> where TValidator : AbstractValidator<TMessage>, new()
    {
        public TValidator Validator => new TValidator();
    }
}