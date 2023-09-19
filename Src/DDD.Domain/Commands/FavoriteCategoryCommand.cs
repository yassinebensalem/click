using System;
using DDD.Domain.Core.Commands;


namespace DDD.Domain.Commands
{
    public abstract class FavoriteCategoryCommand : Command
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
