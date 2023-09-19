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
    public class PromoUserCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewPromoUserCommand, bool>,
        IRequestHandler<UpdatePromoUserCommand, bool>,
        IRequestHandler<RemovePromoUserCommand, bool>
    {
        private readonly IPromoUserRepository _promoUserRepository;
        private readonly IMediatorHandler Bus;

        public PromoUserCommandHandler(IPromoUserRepository promoUserRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _promoUserRepository = promoUserRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewPromoUserCommand message, CancellationToken cancellationToken)
        {
            message.Id = Guid.NewGuid();
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var promoUser = new PromoUser(message.UserId, message.PromotionId, message.BooksTakenCount);

            _promoUserRepository.Add(promoUser);

            if (Commit())
            {
                Bus.RaiseEvent(new PromoUserRegisteredEvent(Guid.NewGuid(), promoUser.UserId, promoUser.PromotionId, promoUser.BooksTakenCount));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdatePromoUserCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var existingPromoUser = _promoUserRepository.GetById(message.Id);

            if (existingPromoUser != null && existingPromoUser.Id != message.Id)
            {
                if (!existingPromoUser.Equals(message))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The model is already existing."));
                    return Task.FromResult(false);
                }
            }
            existingPromoUser.UserId = message.UserId;
            existingPromoUser.PromotionId = message.PromotionId;
            existingPromoUser.BooksTakenCount = message.BooksTakenCount;

            _promoUserRepository.Update(existingPromoUser);

            if (Commit())
            {
                Bus.RaiseEvent(new PromoUserUpdatedEvent(existingPromoUser.UserId, existingPromoUser.PromotionId, existingPromoUser.BooksTakenCount));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemovePromoUserCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _promoUserRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new PromoUserRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _promoUserRepository.Dispose();
        }
    }
}
