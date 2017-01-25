using System;
using System.Threading.Tasks;
using FluentValidation;

namespace SiteWatchman.Common.Application
{
    public abstract class MessageBase<TMessage, TValidator, TReturn> where TValidator : AbstractValidator<TMessage>, new()
    {
        protected async Task<TReturn> ExecuteAsync(TMessage instance, Func<Task<TReturn>> asyncFunc)
        {
            // validate the message
            ValidateInstance(instance);

            // call the async function
            return await asyncFunc();
        }

        protected TReturn Execute(TMessage instance, Func<TReturn> func)
        {
            // validate the message
            ValidateInstance(instance);

            // call the function
            return func();
        }

        private static void ValidateInstance(TMessage instance)
        {
            // create an instance of the validator
            var validator = new TValidator();

            // validate the command
            validator.ValidateAndThrow(instance);
        }
    }
}