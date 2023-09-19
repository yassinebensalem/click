using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class CartRemovedEvent : Event
    {
        public Guid Id { get; set; }

        public CartRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
