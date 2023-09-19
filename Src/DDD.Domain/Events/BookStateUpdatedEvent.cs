using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Events
{
   public  class BookStateUpdatedEvent : Event
    {
        public BookStateUpdatedEvent (Guid id, BookState Status)
        {
            Id = id;
            this.Status = Status;

        }

        public Guid Id { get; set; }
        public BookState Status { get; set; }
    }
}
