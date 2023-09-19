using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;
namespace DDD.Domain.EventHandlers
{
    public class CommunityEventHandler :
           INotificationHandler<CommunityRegisteredEvent>,
           INotificationHandler<CommunityUpdatedEvent>,
           INotificationHandler<CommunityRemovedEvent>
    {
        public Task Handle(CommunityUpdatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(CommunityRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(CommunityRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}
