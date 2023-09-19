using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
  public  class UpdateCategoryCommand : CategoryCommand
    {
        public UpdateCategoryCommand(Guid id,  string categoryName, bool status, Guid? _ParentId)
        {
            Id = id;
            CategoryName = categoryName;
            Status=status;
            ParentId = _ParentId;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
