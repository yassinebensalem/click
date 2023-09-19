using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RegisterNewFavoriteCategoryCommand : FavoriteCategoryCommand
    {
        public RegisterNewFavoriteCategoryCommand(string _UserId, Guid _CategoryId)
        {
            //Id = id;
            UserId = _UserId;
            CategoryId = _CategoryId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewFavoriteCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
