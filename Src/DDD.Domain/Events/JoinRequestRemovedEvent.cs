using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class JoinRequestRemovedEvent : Event
    {
        public JoinRequestRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
