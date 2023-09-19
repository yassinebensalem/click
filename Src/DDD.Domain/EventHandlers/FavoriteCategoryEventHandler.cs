using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;

namespace DDD.Domain.EventHandlers
{
    public class FavoriteCategoryEventHandler :
        INotificationHandler<FavoriteCategoryRegisteredEvent>,
        INotificationHandler<FavoriteCategoryRemovedEvent>
    {

        public Task Handle(FavoriteCategoryRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(FavoriteCategoryRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}
