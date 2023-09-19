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
    public class PromotionEventHandler : INotificationHandler<PromotionRegisteredEvent>,
                                     INotificationHandler<PromotionBookRegisteredEvent>
    {

        public Task Handle(PromotionRegisteredEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        public Task Handle(PromotionBookRegisteredEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
