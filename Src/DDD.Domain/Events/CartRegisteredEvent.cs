using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class CartRegisteredEvent : Event
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }

        public CartRegisteredEvent(Guid id, string userId, Guid bookId)
        {
            Id = id;
            UserId = userId;
            BookId = bookId;
        }
    }
}
