using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RegisterNewLibraryCommand : LibraryCommand
    {
        public RegisterNewLibraryCommand(Guid _Id, string _UserId, Guid _BookId)
        {
            this.Id = _Id;
            this.UserId = _UserId;
            this.BookId = _BookId; 
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewLibraryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
