using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemoveBookCommand : BookCommand
    {
        public RemoveBookCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
