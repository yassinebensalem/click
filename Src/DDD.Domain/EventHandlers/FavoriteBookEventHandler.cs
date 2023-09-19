using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;

namespace DDD.Domain.EventHandlers
{
    public class FavoriteBookEventHandler :
        INotificationHandler<FavoriteBookRegisteredEvent>, 
        INotificationHandler<FavoriteBookRemovedEvent>
    { 

        public Task Handle(FavoriteBookRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(FavoriteBookRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}
