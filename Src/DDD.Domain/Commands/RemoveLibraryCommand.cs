using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemoveLibraryCommand : LibraryCommand
    {
        public RemoveLibraryCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveLibraryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
