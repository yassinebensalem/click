using System;
using System.Collections.Generic;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class WalletTransactionEvent : Event
    {
        public WalletTransactionEvent(ICollection<Guid> userIds, Guid? communityId, float amount, bool status)
        {
            CommunityId = communityId;
            UserIds = userIds;
            Amount = amount;
            Status = status;

        }

        public Guid? CommunityId { get; set; }
        public ICollection<Guid> UserIds { get; set; }
        public float Amount { get; set; }
        public bool Status { get; set; }

    }
}
