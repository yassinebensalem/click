using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class PromoUser : EntityAudit
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Guid PromotionId { get; set; }
        [ForeignKey("PromotionId")]
        public Promotion Promotion { get; set; }
        public int BooksTakenCount { get; set; }

        public PromoUser(string userId, ApplicationUser user, Guid promotionId, Promotion promotion, int booksTakenCount)
        {
            UserId = userId;
            User = user;
            PromotionId = promotionId;
            Promotion = promotion;
            BooksTakenCount = booksTakenCount;
        }

        public PromoUser(string userId, Guid promotionId, int booksTakenCount)
        {
            UserId = userId;
            PromotionId = promotionId;
            BooksTakenCount = booksTakenCount;
        }
    }
}
