using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class BookRemovedEvent : Event
    {
        public BookRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
