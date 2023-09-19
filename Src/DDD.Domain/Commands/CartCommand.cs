using System;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
    public abstract class CartCommand : Command
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
    }
}
