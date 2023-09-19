using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class AuthorRemovedEvent : Event
    {
        public AuthorRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
