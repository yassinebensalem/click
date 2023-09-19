using System;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
   public abstract class LanguageCommand : Command
    {
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public string CodeAlpha2 { get; set; }
        public string CodeAlpha3 { get; set; }
        public string Code { get; set; }

    }
}
