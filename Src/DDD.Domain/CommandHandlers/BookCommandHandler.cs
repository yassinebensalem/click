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
    public class BookCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewBookCommand, bool>,
        IRequestHandler<UpdateBookCommand, bool>,
        IRequestHandler<RemoveBookCommand, bool>,
        IRequestHandler<UpdateBookStateCommand, bool>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMediatorHandler Bus;

        public BookCommandHandler(IBookRepository bookRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _bookRepository = bookRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var book = new Book(Guid.NewGuid(), message.Title, message.PageNumbers, message.CoverPath, message.Price, message.AuthorId,
                message.PublisherId, message.CategoryId, message.CountryId, message.LanguageId, message.Description,
                message.PublicationDate, message.ISBN, message.ISSN, message.EISBN, message.PDFPath, message.Status);
            book.StatusUpdateDate = DateTime.Now;

            //if (_bookRepository.GetByEmail(book.Email) != null)
            //{
            //    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The book e-mail has already been taken."));
            //    return Task.FromResult(false);
            //}

            _bookRepository.Add(book);

            if (Commit())
            {
                Bus.RaiseEvent(new BookRegisteredEvent(book.Id, book.Title, book.PageNumbers, book.CoverPath, book.Price,
                    book.Description, book.PublicationDate, book.AuthorId, book.PublisherId, book.CategoryId,
                    book._CountryId, book._LanguageId, book.ISBN, book.ISSN, book.EISBN, book.PDFPath, book.Status));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var book = new Book(message.Id, message.Title, message.PageNumbers, message.CoverPath, message.Price,
                message.AuthorId, message.PublisherId, message.CategoryId, message.CountryId, message.LanguageId,
                message.Description, message.PublicationDate, message.ISBN, message.ISSN, message.EISBN,
                message.PDFPath, message.Status);

            var existingBook = _bookRepository.GetById(book.Id);

            existingBook.Title = message.Title;
            existingBook.PageNumbers = message.PageNumbers;
            existingBook.Price = message.Price;
            existingBook.AuthorId = message.AuthorId;
            existingBook.PublisherId = message.PublisherId;
            existingBook.CategoryId = message.CategoryId;
            existingBook._CountryId = message.CountryId;
            existingBook._LanguageId = message.LanguageId;
            existingBook.Description = message.Description;
            existingBook.PublicationDate = message.PublicationDate;
            existingBook.ISBN = message.ISBN;
            existingBook.ISSN = message.ISSN;
            existingBook.EISBN = message.EISBN;
            existingBook.CoverPath = message.CoverPath != null ? message.CoverPath : existingBook.CoverPath;
            existingBook.PDFPath = message.PDFPath != null ? message.PDFPath : existingBook.PDFPath;
            ////existingBook.IsFree = message.IsFree;
            if (existingBook.Status != message.Status) existingBook.StatusUpdateDate = DateTime.Now;
            existingBook.Status = message.Status;
            existingBook.UpdatedAt = DateTime.Now;

            if (existingBook != null && existingBook.Id != book.Id)
            {
                if (!existingBook.Equals(book))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The book is already existing."));
                    return Task.FromResult(false);
                }
            }

            _bookRepository.Update(existingBook);

            if (Commit())
            {
                Bus.RaiseEvent(new BookUpdatedEvent(book.Id, book.Title, book.PageNumbers, book.CoverPath, book.Price, book.Description,
                    book.PublicationDate, book.AuthorId, book.PublisherId, book.CategoryId, book._CountryId, book._LanguageId,
                    book.ISBN, book.ISSN, book.EISBN, book.PDFPath, book.Status));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _bookRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new BookRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateBookStateCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }
            var book = new Book(message.Id, message.Status);
            var existingBook = _bookRepository.GetById(book.Id);

            existingBook.Status = message.Status;
            //if (message.Status == Common.Constants.State.BookState.published) existingBook.PublicationDate = DateTime.Now;
            existingBook.StatusUpdateDate = DateTime.Now;

            if (existingBook != null && existingBook.Id != book.Id)
            {
                if (!existingBook.Equals(book))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The book is already existing."));
                    return Task.FromResult(false);
                }

            }

            _bookRepository.Update(existingBook);

            if (Commit())
            {
                Bus.RaiseEvent(new BookStateUpdatedEvent(book.Id, book.Status));
            }

            return Task.FromResult(true);

        }

        public void Dispose()
        {
            _bookRepository.Dispose();
        }
    }
}
