using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class PrizeBookRegisteredEvent : Event
    {
        public PrizeBookRegisteredEvent(Guid id, Guid prizeId, Guid bookId, string edition)

        {
            Id = id;
            PrizeId = prizeId;
            BookId = bookId;
            Edition = edition;
        }



        public Guid Id { get; set; }
        public Guid PrizeId { get; set; }
        public Guid BookId { get; set; }
        public string Edition { get; set; }

    }
}
