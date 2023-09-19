using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class CommunityValidation<T> : AbstractValidator<T> where T : CommunityCommand
    {
        protected void ValidateCommunityName()
        {
            RuleFor(c => c.CommunityName)
                .NotEmpty().WithMessage("Please ensure you have entered the Community Name")
                .Length(2, 150).WithMessage("The Community Name must have between 2 and 150 characters");
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
