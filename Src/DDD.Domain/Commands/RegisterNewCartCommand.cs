using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RegisterNewCartCommand : CartCommand
    {
        public RegisterNewCartCommand(string _UserId, Guid _BookId)
        {
            //Id = id;
            UserId = _UserId;
            BookId = _BookId;
        }
        public override bool IsValid()
        {
            ValidationResult = new RegisterNewCartCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
