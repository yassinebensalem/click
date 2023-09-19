using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;
namespace DDD.Domain.EventHandlers
{
    public class WalletTransactionEventHandler :
           INotificationHandler<WalletTransactionEvent>,
           INotificationHandler<WalletTransactionRefilledEvent>,
           INotificationHandler<WalletTransactionWithdrawnEvent>
    {
        public Task Handle(WalletTransactionRefilledEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(WalletTransactionWithdrawnEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(WalletTransactionEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}
