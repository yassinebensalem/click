using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class CommunityMemberEvent : Event
    {
        public CommunityMemberEvent(Guid communityId, string memberId, bool isCommunityAdmin, bool status)
        {
            CommunityId = communityId;
            MemberId = memberId;
            IsCommunityAdmin = isCommunityAdmin;
            Status = status;

        }

        public Guid CommunityId { get; set; }
        public string MemberId { get; set; }
        public bool IsCommunityAdmin { get; set; }
        public bool Status { get; set; }

    }
}
