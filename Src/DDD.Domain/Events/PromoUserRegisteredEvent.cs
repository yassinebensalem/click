using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Events
{
    public class PromoUserRegisteredEvent : Event
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid PromotionId { get; set; }
        public int BooksTakenCount { get; set; }

        public PromoUserRegisteredEvent(Guid id, string userId, Guid promotionId, int booksTakenCount)
        {
            Id = id;
            UserId = userId;
            PromotionId = promotionId;
            BooksTakenCount = booksTakenCount;
        }
    }
}
