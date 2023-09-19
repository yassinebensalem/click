using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;


namespace DDD.Domain.EventHandlers
{
    public class CartEventHandler :
        INotificationHandler<CartRegisteredEvent>,
        INotificationHandler<CartRemovedEvent>
    {
        public Task Handle(CartRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(CartRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}
