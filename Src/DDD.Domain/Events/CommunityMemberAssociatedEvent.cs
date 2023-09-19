using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class CommunityMemberAssociatedEvent : CommunityMemberEvent
    {
        public CommunityMemberAssociatedEvent(Guid communityId, string memberId, bool isCommunityAdmin, bool status) : base(communityId, memberId, isCommunityAdmin, status)
        {

        }
        
    }
}
