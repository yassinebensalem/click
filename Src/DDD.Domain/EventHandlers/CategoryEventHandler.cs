using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;
namespace DDD.Domain.EventHandlers
{
 public   class CategoryEventHandler :
        INotificationHandler<CategoryRegisteredEvent>,
        INotificationHandler<CategoryUpdatedEvent>,
        INotificationHandler<CategoryRemovedEvent>
    {
        public Task Handle(CategoryUpdatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(CategoryRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(CategoryRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}
