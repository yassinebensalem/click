using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class PromotionBookRegisteredEvent : Event
    {
        public PromotionBookRegisteredEvent(Guid id, Guid promotionId, Guid bookId   )

        {
            Id = id;
            PromotionId = promotionId;
            BookId = bookId; 
        }
          
        public Guid Id { get; set; }
        public Guid PromotionId { get; set; }
        public Guid BookId { get; set; } 
    }
}
