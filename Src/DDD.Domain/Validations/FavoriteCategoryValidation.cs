using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public class FavoriteCategoryValidation<T> : AbstractValidator<T> where T : FavoriteCategoryCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }
        protected void ValidateUserId()
        {
            RuleFor(c => c.UserId)
                .NotEmpty();
        }
        protected void ValidateCategoryId()
        {
            RuleFor(c => c.CategoryId)
                .NotEqual(Guid.Empty);
        }
    }
}
