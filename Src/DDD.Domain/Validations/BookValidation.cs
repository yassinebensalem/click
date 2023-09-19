using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class BookValidation<T> : AbstractValidator<T> where T : BookCommand
    {
        protected void ValidateTitle()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Please ensure you have entered the Title")
                .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }

        protected void ValidatePublicationDate()
        {
            RuleFor(c => c.PublicationDate)
                .NotEmpty()
                .WithMessage("The Publication Date must not be empty");
        }

        protected void ValidatePageNumbers()
        {
            RuleFor(c => c.PageNumbers)
                .NotEmpty()
                .WithMessage("The Page Numbers must be Greater than zero")
                .Must(GreaterThanZero);
        }
        protected void ValidateCoverPath()
        {
            RuleFor(c => c.CoverPath)
                .NotEmpty().WithMessage("Please ensure that you selected a Cover Picture for the book");
        }
        protected void ValidatePrice()
        {
            RuleFor(c => c.Price)
                .NotEmpty()
                .WithMessage("The Book Price must be Greater than zero")
                .Must(PriceGreaterThanZero);
        }
        protected void ValidateDescription()
        {
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("The Description must not be empty");
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        protected void ValidateISBN()
        {
            RuleFor(c => c.ISBN)
                .NotEmpty().WithMessage("The ISBN must not be empty");
        }
        protected void ValidateISSN()
        {
            RuleFor(c => c.ISSN)
                .NotEmpty().WithMessage("The ISSN must not be empty");
        }
        protected void ValidateEISBN()
        {
            RuleFor(c => c.EISBN)
                .NotEmpty().WithMessage("The EISBN must not be empty");
        }
        protected void ValidatePDFPath()
        {
            RuleFor(c => c.PDFPath)
                .NotEmpty().WithMessage("The PDF File must not be empty");
        }

        protected static bool HaveMinimumAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now.AddYears(-18);
        }

        protected static bool GreaterThanZero(int PageNumbers)
        {
            return PageNumbers > 0;
        }
        protected static bool PriceGreaterThanZero(double Price)
        {
            return Price > 0;
        }
    }
}
