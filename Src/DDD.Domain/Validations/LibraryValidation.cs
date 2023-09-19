using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public class LibraryValidation<T> : AbstractValidator<T> where T : LibraryCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        protected void ValidateUserId()
        {
            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("The UserId must not be empty");
        }

        protected void ValidateBookId()
        {
            RuleFor(c => c.BookId)
                .NotEqual(Guid.Empty);
        }
    }
}
