using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
  public  class CommunityRemovedEvent : Event
    {
        public CommunityRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
