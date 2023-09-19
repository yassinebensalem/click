using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemovePromoUserCommand : PromoUserCommand
    {
        public RemovePromoUserCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemovePromoUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
