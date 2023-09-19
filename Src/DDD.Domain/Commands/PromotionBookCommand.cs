using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
    public abstract class PromotionBookCommand : Command
    {
        public Guid Id { get; set; }
        public Guid PromotionId { get; set; }
        public Guid BookId { get; set; } 
    }
}
