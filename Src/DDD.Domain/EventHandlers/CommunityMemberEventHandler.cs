using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;
namespace DDD.Domain.EventHandlers
{
    public class CommunityMemberEventHandler :
           INotificationHandler<CommunityMemberEvent>,
           INotificationHandler<CommunityMemberAssociatedEvent>,
           INotificationHandler<CommunityMemberDissociatedEvent>
    {
        public Task Handle(CommunityMemberAssociatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(CommunityMemberDissociatedEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(CommunityMemberEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}
