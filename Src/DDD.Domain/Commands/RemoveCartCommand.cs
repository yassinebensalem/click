using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemoveCartCommand : CartCommand
    {
        public RemoveCartCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveCartCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
