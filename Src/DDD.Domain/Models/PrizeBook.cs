using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
   public class PrizeBook : EntityAudit
    {
        public Guid PrizeId { get; set; }
        [ForeignKey("PrizeId")]
        public Prize Prize  { get; set; }
        public Guid BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public string  Edition  { get; set; }


        public PrizeBook(Guid id, Guid prizeId, Guid bookId, string edition)
        {
            Id = id;
            PrizeId = prizeId;
            BookId = bookId;
            Edition = edition;

        }

        public PrizeBook()
        {

        }


    }

   
}
