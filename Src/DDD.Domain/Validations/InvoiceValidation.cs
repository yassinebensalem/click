using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public class InvoiceValidation<T> : AbstractValidator<T> where T : InvoiceCommand
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

        protected void ValidatePrice()
        {
            RuleFor(c => c.Price)
                .NotEmpty()
                .WithMessage("The Book Price must be Greater than zero")
                .Must(PriceGreaterThanZero);
        }

        protected void ValidatePublicationDate()
        {
            RuleFor(c => c.Date)
                .NotEmpty()
                .WithMessage("The Invoice Date must not be empty");
        }

        protected static bool PriceGreaterThanZero(double Price)
        {
            return Price > 0;
        }
    }
}
