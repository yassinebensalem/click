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
    public class FavoriteCategoryCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewFavoriteCategoryCommand, bool>,
        IRequestHandler<RemoveFavoriteCategoryCommand, bool>
    {
        private readonly IFavoriteCategoryRepository _favoriteCategoryRepository;
        private readonly IMediatorHandler Bus;

        public FavoriteCategoryCommandHandler(IFavoriteCategoryRepository favoriteCategoryRepository,
                                            IMediatorHandler bus, IUnitOfWork uow,
                                            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _favoriteCategoryRepository = favoriteCategoryRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewFavoriteCategoryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var favoriteCategory = new FavoriteCategory(Guid.NewGuid(), message.UserId, message.CategoryId);

            _favoriteCategoryRepository.Add(favoriteCategory);

            if (Commit())
            {
                Bus.RaiseEvent(new FavoriteCategoryRegisteredEvent(favoriteCategory.Id, favoriteCategory.UserId, favoriteCategory.CategoryId));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveFavoriteCategoryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _favoriteCategoryRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new CategoryRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _favoriteCategoryRepository.Dispose();
        }
    }
}
