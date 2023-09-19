using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
  public  class RegisterNewCategoryCommand : CategoryCommand
    {
        public RegisterNewCategoryCommand(Guid _Id, string _CategoryName,bool _Status, Guid? _ParentId)
        {
            Id = _Id;
            CategoryName = _CategoryName;
            Status = _Status;
            ParentId = _ParentId;
        }

        public RegisterNewCategoryCommand(string _CategoryName, bool _Status,Guid? _ParentId)
        {
            CategoryName = _CategoryName;
            Status = _Status;
            ParentId = _ParentId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
