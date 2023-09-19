using System;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
    public abstract class CategoryCommand : Command
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public bool Status { get; set; }
        public Guid? ParentId { get; set; } 
    }
}
