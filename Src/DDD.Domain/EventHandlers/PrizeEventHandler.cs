using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using MediatR;

namespace DDD.Domain.EventHandlers
{
    public class PrizeEventHandler : INotificationHandler<PrizeRegisteredEvent>,
                                     INotificationHandler<PrizeBookRegisteredEvent>
    {

        public Task Handle(PrizeRegisteredEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

       
        public Task Handle(PrizeBookRegisteredEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
