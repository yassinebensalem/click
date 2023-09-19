using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class FavoriteBookValidation<T> : AbstractValidator<T> where T : FavoriteBookCommand
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
        protected void ValidateBookId()
        {
            RuleFor(c => c.BookId)
                .NotEqual(Guid.Empty);
        }
    }
}
