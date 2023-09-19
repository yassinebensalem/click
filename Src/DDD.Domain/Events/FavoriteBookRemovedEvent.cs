using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class FavoriteBookRemovedEvent : Event
    {
        public FavoriteBookRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
