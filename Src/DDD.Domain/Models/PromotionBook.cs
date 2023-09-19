using System;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class PromotionBook : EntityAudit
    {
        public Guid PromotionId { get; set; }
        [ForeignKey("PromotionId")]
        public Promotion Promotion { get; set; }
        public Guid BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public PromotionBook()
        {
        }

        public PromotionBook(Guid id, Guid promotionId, Guid bookId)
        {
            Id = id;
            PromotionId = promotionId;
            BookId = bookId;
        }
    }
}
