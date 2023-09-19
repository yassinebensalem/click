using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Events;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using System.IO;
using MediatR;

namespace DDD.Domain.CommandHandlers
{
    public class AuthorCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewAuthorCommand, bool>,
       IRequestHandler<UpdateAuthorCommand, bool>,
       IRequestHandler<RemoveAuthorCommand, bool>

    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMediatorHandler Bus;

        public AuthorCommandHandler(IAuthorRepository authorRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _authorRepository = authorRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewAuthorCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var author = new Author(Guid.NewGuid(), message.FirstName, message.LastName, message.Email, message.PhoneNumber, message.CountryId, message.Biography,
                message.Birthdate, message.PhotoPath, Guid.NewGuid() );


            //if (_bookRepository.GetByEmail(book.Email) != null)
            //{
            //    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The book e-mail has already been taken."));
            //    return Task.FromResult(false);
            //}

            _authorRepository.Add(author);

            if (Commit())
            {
                Bus.RaiseEvent(new AuthorRegisteredEvent(author.Id,author.FirstName, author.LastName, author.Email, author.PhoneNumber, author.CountryId ,author.Biography,
                author.Birthdate, author.PhotoPath));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateAuthorCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var currentUser = _authorRepository.GetById(message.Id);
            if (currentUser != null)
            {
                //_authorRepository.Dispose();
                currentUser.FirstName = message.FirstName;
                currentUser.LastName = message.LastName;
                currentUser.Email = message.Email;
                currentUser.PhoneNumber = message.PhoneNumber;
                currentUser.Biography = message.Biography;
                currentUser.Birthdate = message.Birthdate;
                currentUser.PhotoPath = message.PhotoPath;
                currentUser.CountryId = message.CountryId;

                _authorRepository.Update(currentUser);

                if (Commit())
                {
                    Bus.RaiseEvent(new AuthorUpdatedEvent(currentUser.Id, currentUser.FirstName, currentUser.LastName,
                        currentUser.Email, currentUser.PhoneNumber, currentUser.CountryId, currentUser.Biography,
                        currentUser.Birthdate, currentUser.PhotoPath));
                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveAuthorCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _authorRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new AuthorRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _authorRepository.Dispose();
        }
    }
}

