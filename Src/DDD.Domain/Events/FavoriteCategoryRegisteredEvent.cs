using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class FavoriteCategoryRegisteredEvent : Event
    {
        public FavoriteCategoryRegisteredEvent(Guid id, string userId, Guid bookId)
        {
            Id = id;
            UserId = userId;
            CategoryId = bookId;
        }

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
