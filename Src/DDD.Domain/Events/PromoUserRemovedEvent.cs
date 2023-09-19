using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class PromoUserRemovedEvent : Event
    {
        public PromoUserRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
