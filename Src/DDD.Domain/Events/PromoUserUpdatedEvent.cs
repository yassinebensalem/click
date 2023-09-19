using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class PromoUserUpdatedEvent : Event
    { 
        public string UserId { get; set; }
        public Guid PromotionId { get; set; }
        public int BooksTakenCount { get; set; }

        public PromoUserUpdatedEvent(string userId, Guid promotionId, int booksTakenCount)
        {
            UserId = userId;
            PromotionId = promotionId;
            BooksTakenCount = booksTakenCount;
        }
    }
}
