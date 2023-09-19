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
  public  class CategoryCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewCategoryCommand, bool>,
        IRequestHandler<UpdateCategoryCommand, bool>,
        IRequestHandler<RemoveCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediatorHandler Bus;

        public CategoryCommandHandler(ICategoryRepository categoryRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _categoryRepository = categoryRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewCategoryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var category = new Category(Guid.NewGuid(), message.CategoryName, message.Status,message.ParentId);

            //if (_bookRepository.GetByEmail(book.Email) != null)
            //{
            //    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The book e-mail has already been taken."));
            //    return Task.FromResult(false);
            //}

            _categoryRepository.Add(category);

            if (Commit())
            {
                Bus.RaiseEvent(new CategoryRegisteredEvent(category.Id, category.CategoryName, category.Status,category.ParentId));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateCategoryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var category = new Category(message.Id, message.CategoryName, message.Status, message.ParentId);
            var existingCategory = _categoryRepository.GetById(category.Id);
            existingCategory.CategoryName = message.CategoryName;
            existingCategory.Status = message.Status;
            existingCategory.ParentId = message.ParentId;



            //if (existingCategory != null && existingCategory.Id != category.Id)
            //{
            //    if (!existingCategory.Equals(category))
            //    {
            //        Bus.RaiseEvent(new DomainNotification(message.MessageType, "This category is already existing."));
            //        return Task.FromResult(false);
            //    }
            //}

            _categoryRepository.Update(existingCategory);

            if (Commit())
            {
                Bus.RaiseEvent(new CategoryUpdatedEvent(category.Id, category.CategoryName,category.Status));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveCategoryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _categoryRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new CategoryRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _categoryRepository.Dispose();
        }
    }
}
