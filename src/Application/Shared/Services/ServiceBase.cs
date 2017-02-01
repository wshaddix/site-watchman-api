using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;
using SiteWatchman.Common.Domain;
using SiteWatchman.Common.Persistance;

namespace SiteWatchman.Application.Shared.Services
{
    public abstract class ServiceBase : IService
    {
        private readonly IDatabase _database;

        public MessageContext MessageContext { get; set; }

        protected IDatabase Database => _database;

        protected ServiceBase(IDatabase database)
        {
            _database = database;
        }

        protected TReturn Execute<TMessage, TValidator, TReturn>(TMessage instance, Func<TReturn> func)
                                                                where TMessage : MessageBase<TMessage, TValidator>, new()
                                                                where TValidator : AbstractValidator<TMessage>, new()
        {
            // validate that the message context is not null
            if (null == MessageContext) throw new MessageContextMissingException();

            // validate the message
            instance.Validator.ValidateAndThrow(instance);

            // call the async function
            return func();
        }

        protected async Task<TReturn> ExecuteAsync<TMessage, TValidator, TReturn>(TMessage instance, Func<Task<TReturn>> asyncFunc)
                                                                where TMessage : MessageBase<TMessage, TValidator>, new()
                                                                where TValidator : AbstractValidator<TMessage>, new()
        {
            // validate that the message context is not null
            if (null == MessageContext) throw new MessageContextMissingException();

            // validate the message
            instance.Validator.ValidateAndThrow(instance);

            // call the async function
            return await asyncFunc().ConfigureAwait(false);
        }

        protected async Task ExecuteAsyncNoReturn<TMessage, TValidator>(TMessage instance, Func<Task> asyncFunc)
                                                                where TMessage : MessageBase<TMessage, TValidator>
                                                                where TValidator : AbstractValidator<TMessage>, new()
        {
            // validate that the message context is not null
            if (null == MessageContext) throw new MessageContextMissingException();

            // validate the message
            instance.Validator.ValidateAndThrow(instance);

            // call the async function
            await asyncFunc().ConfigureAwait(false);
        }

        protected T Get<T>(Expression<Func<T, bool>> where) where T : Entity
        {
            return Database.Get(where);
        }

        protected void Publish(DomainEventBase @event)
        {
            // TODO: Use MediatR or MicroBus or some type of fire and forget notifier
        }
    }
}