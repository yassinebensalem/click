using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;

namespace DDD.Domain.EventHandlers
{
    public class BookEventHandler :
        INotificationHandler<BookRegisteredEvent>,
        INotificationHandler<BookUpdatedEvent>,
        INotificationHandler<BookRemovedEvent>,
          INotificationHandler<BookStateUpdatedEvent>



    {
        public Task Handle(BookUpdatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(BookRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(BookRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }

        public Task Handle(BookStateUpdatedEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
