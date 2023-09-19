using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class FavoriteCategoryRemovedEvent : Event
    {
        public FavoriteCategoryRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
