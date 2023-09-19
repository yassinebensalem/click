using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemoveFavoriteBookCommand : FavoriteBookCommand
    {
        public RemoveFavoriteBookCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveFavoriteBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
