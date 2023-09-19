using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemoveAuthorCommand : AuthorCommand
    {
        public RemoveAuthorCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveAuthorCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
