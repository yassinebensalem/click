using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RegisterNewFavoriteBookCommand : FavoriteBookCommand
    {
        public RegisterNewFavoriteBookCommand(string _UserId, Guid _BookId)
        {
            //Id = id;
            UserId = _UserId;
            BookId = _BookId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewFavoriteBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
