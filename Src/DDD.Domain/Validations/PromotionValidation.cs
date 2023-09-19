using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class PromotionValidation<T> : AbstractValidator<T> where T : PromotionCommand
    {

        protected void ValidateId()
        {
            RuleFor(c => c.Id);
        }

        protected void ValidateName()
        {
            RuleFor(c => c.Name);
        }

        protected void ValidateDescription()
        {
            RuleFor(c => c.Description);
        }

        protected void ValidateCountryId()
        {
            RuleFor(c => c.CountryId);
        }

        protected void ValidateStartDate()
        {
            RuleFor(c => c.StartDate)
                .NotEmpty()
                .WithMessage("The Date must not be empty");
        }
        protected void ValidateEndDate()
        {
            RuleFor(c => c.EndDate)
                .NotEmpty()
                .WithMessage("The Date must not be empty");
        }
    }
}
