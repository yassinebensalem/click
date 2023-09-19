using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class CategoryValidation<T> : AbstractValidator<T> where T : CategoryCommand
    {
        protected void ValidateCategoryName()
        {
            RuleFor(c => c.CategoryName)
                .NotEmpty().WithMessage("Please ensure you have entered the Category Name")
                .Length(2, 150).WithMessage("The Category Name must have between 2 and 150 characters");
        }
        protected void ValidateStatus()
        {
            RuleFor(c => c.Status);
                
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

    }
}
