using System;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
   public  class CategoryRegisteredEvent : Event
    {
        public CategoryRegisteredEvent(Guid categoryId, string categoryName,bool status,Guid? _ParentId)
        {
            Id = categoryId;
            CategoryName = categoryName;
            Status = status;
            ParentId = _ParentId;

        }

        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public bool Status { get; set; }
        public Guid? ParentId { get; set; }

    }
}
