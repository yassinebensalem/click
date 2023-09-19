using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemoveFavoriteCategoryCommand : FavoriteCategoryCommand
    {
        public RemoveFavoriteCategoryCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveFavoriteCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
