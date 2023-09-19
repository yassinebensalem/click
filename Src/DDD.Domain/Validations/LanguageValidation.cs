using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
   public abstract class LanguageValidation<T> : AbstractValidator<T> where T : LanguageCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.LanguageId)
                .NotEqual(Guid.Empty);
        }
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the NAME")
                .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }
        protected void ValidateCodeAlpha2()
        {
            RuleFor(c => c.CodeAlpha2)
                .NotEmpty().WithMessage("Please ensure you have entered the CODE")
                .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }
        protected void ValidateCodeAlpha3()
        {
            RuleFor(c => c.CodeAlpha3)
                .NotEmpty().WithMessage("Please ensure you have entered the CODE")
                .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }
        protected void ValidateCode()
        {
            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Please ensure you have entered the CODE")
                .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }
    }
}
