using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class JoinRequestValidation <T> : AbstractValidator<T> where T : JoinRequestCommand
    {
        protected void ValidateFirstName()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("Please ensure you have entered the FirstName");
               // .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }

        protected void ValidateLastName()
        {
            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("Please ensure you have entered the LastName");
               // .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }
        protected void ValidateEmail()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Please ensure you have entered the Email");
            // .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }
        protected void ValidatePhoneNumber()
        {
            RuleFor(c => c.PhoneNumber)
                .NotEmpty().WithMessage("Please ensure you have entered the PhoneNumber");
            // .Length(2, 150).WithMessage("The Title must have between 2 and 150 characters");
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
