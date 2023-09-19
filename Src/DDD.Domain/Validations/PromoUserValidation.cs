using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class PromoUserValidation<T> : AbstractValidator<T> where T : PromoUserCommand
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
        protected void ValidatePromotionId()
        {
            RuleFor(c => c.PromotionId)
                .NotEqual(Guid.Empty);
        }
    }
}
