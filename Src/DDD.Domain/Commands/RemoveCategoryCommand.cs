using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
  public  class RemoveCategoryCommand : CategoryCommand
    {
        public RemoveCategoryCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
