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
    public class JoinRequestEventHandler :

        INotificationHandler<JoinRequestRegisteredEvent>,
        INotificationHandler<JoinRequestUpdatedEvent>,
        INotificationHandler<JoinRequestRemovedEvent>
    {


        public Task Handle(JoinRequestRegisteredEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(JoinRequestUpdatedEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(JoinRequestRemovedEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }


}

