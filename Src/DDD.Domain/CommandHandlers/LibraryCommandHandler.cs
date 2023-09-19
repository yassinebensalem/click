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
    public class LibraryCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewLibraryCommand, bool>,
        IRequestHandler<UpdateLibraryCommand, bool>,
        IRequestHandler<RemoveLibraryCommand, bool>

    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMediatorHandler Bus;

        public LibraryCommandHandler(ILibraryRepository libraryRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _libraryRepository = libraryRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewLibraryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var library = new Library(message.UserId, message.BookId);

            _libraryRepository.Add(library);

            if (Commit())
            {
                Bus.RaiseEvent(new LibraryAddedEvent(Guid.NewGuid(), library.UserId, library.BookId));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateLibraryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var existingLibrary = _libraryRepository.GetById(message.Id);
            existingLibrary.CurrentPage = message.CurrentPage;

            if (existingLibrary != null && existingLibrary.Id != message.Id)
            {
                if (!existingLibrary.Equals(message))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The .... is already existing."));
                    return Task.FromResult(false);
                }
            }

            _libraryRepository.Update(existingLibrary);

            if (Commit())
            {
               // Bus.RaiseEvent(new PromotionUpdatedEvent(library.Id, promotion.Name, promotion.PromotionType, promotion.Percentage, promotion.StartDate, promotion.EndDate, promotion.Description, promotion.ImagePath, promotion.CountryId));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveLibraryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _libraryRepository.Remove(message.Id);

            if (Commit())
            {
                //Bus.RaiseEvent(new CartRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _libraryRepository.Dispose();
        }
    }
}
