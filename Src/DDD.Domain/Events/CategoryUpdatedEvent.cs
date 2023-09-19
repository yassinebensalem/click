using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
public    class CategoryUpdatedEvent : Event
    {
        public CategoryUpdatedEvent(Guid categoryId, string categoryName,bool status)
        {
            Id = categoryId;
            CategoryName = categoryName;
            Status = status;


        }

        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public bool Status { get; set; }

    }
}
