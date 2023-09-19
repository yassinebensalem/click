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
    public class PrizeBookEventHandler : INotificationHandler<PrizeBookRegisteredEvent>
    {
        public Task Handle(PrizeBookRegisteredEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
