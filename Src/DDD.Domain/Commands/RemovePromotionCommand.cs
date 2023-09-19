using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemovePromotionCommand : PromotionCommand
    {
        public RemovePromotionCommand(Guid id)
        {
            Id = id;// this.Id;
            AggregateId = id;//this.Id;

        }


        public override bool IsValid()
        {
            ValidationResult = new RemovePromotionCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
