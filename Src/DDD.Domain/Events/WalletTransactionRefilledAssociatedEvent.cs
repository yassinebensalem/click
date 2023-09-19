using System;
using System.Collections.Generic;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class WalletTransactionRefilledEvent : WalletTransactionEvent
    {
        public WalletTransactionRefilledEvent(ICollection<Guid> userIds, Guid? communityId, float amount, bool status) : base(userIds, communityId, amount, status)
        {

        }
        
    }
}
