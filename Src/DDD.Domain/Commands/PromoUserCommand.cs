using System;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
       public abstract class PromoUserCommand : Command
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid PromotionId { get; set; }
        public int BooksTakenCount { get; set; } 
    }
}
