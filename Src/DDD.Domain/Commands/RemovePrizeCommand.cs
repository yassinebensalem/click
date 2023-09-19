using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemovePrizeCommand : PrizeCommand
    {
        public RemovePrizeCommand(Guid id)
        {
            Id = id;
            AggregateId = this.Id;

        }


        public override bool IsValid()
        {
            ValidationResult = new RemovePrizeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
