using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class InvoiceAddedEvent : Event
    {
        public InvoiceAddedEvent(Guid id, string userId, DateTime date, Guid bookId, double price)
        {
            Id = id;
            UserId = userId;
            Date = date;
            BookId = bookId;
            Price = price;
        }

        public Guid Id { get; set; }
        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public Guid BookId { get; set; }

        public double Price { get; set; }
    }
}
