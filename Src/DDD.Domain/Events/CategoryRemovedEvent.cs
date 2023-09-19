using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
  public  class CategoryRemovedEvent : Event
    {
        public CategoryRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
