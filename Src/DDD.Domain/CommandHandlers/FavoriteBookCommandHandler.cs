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
    public class FavoriteBookCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewFavoriteBookCommand, bool>, 
        IRequestHandler<RemoveFavoriteBookCommand, bool>
    {
        private readonly IFavoriteBookRepository _favoriteBookRepository;
        private readonly IMediatorHandler Bus;

        public FavoriteBookCommandHandler(IFavoriteBookRepository favoriteBookRepository,
                                            IMediatorHandler bus, IUnitOfWork uow,
                                            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _favoriteBookRepository = favoriteBookRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewFavoriteBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var favoriteBook = new FavoriteBook(Guid.NewGuid(), message.UserId, message.BookId);

            _favoriteBookRepository.Add(favoriteBook);

            if (Commit())
            {
                Bus.RaiseEvent(new FavoriteBookRegisteredEvent(favoriteBook.Id, favoriteBook.UserId, favoriteBook.BookId));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveFavoriteBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _favoriteBookRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new BookRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _favoriteBookRepository.Dispose();
        }
    }
}
