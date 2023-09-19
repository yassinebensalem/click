using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Events;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using MediatR;

namespace DDD.Domain.CommandHandlers
{
    public class CartCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewCartCommand, bool>,
        IRequestHandler<RemoveCartCommand, bool>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMediatorHandler Bus;

        public CartCommandHandler(ICartRepository cartRepository, IMediatorHandler bus, IUnitOfWork uow,
                                            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _cartRepository = cartRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewCartCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var cart = new Cart(Guid.NewGuid(), message.UserId, message.BookId);

            _cartRepository.Add(cart);

            if (Commit())
            {
                Bus.RaiseEvent(new CartRegisteredEvent(cart.Id, cart.UserId, cart.BookId));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveCartCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _cartRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new CartRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _cartRepository.Dispose();
        }
    }
}
