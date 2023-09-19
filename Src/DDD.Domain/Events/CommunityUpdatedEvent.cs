using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class CommunityUpdatedEvent : Event
    {
        public CommunityUpdatedEvent(Guid communityId, string communityName, bool status)
        {
            Id = communityId;
            CommunityName = communityName;
            Status = status;


        }

        public Guid Id { get; set; }
        public string CommunityName { get; set; }
        public bool Status { get; set; }

    }
}
