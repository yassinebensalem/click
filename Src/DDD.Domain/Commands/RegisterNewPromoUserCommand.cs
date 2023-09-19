using System;
using DDD.Domain.Validations;
using FluentValidation;

namespace DDD.Domain.Commands
{
    public class RegisterNewPromoUserCommand : PromoUserCommand
    {
        public RegisterNewPromoUserCommand(string _UserId, Guid _PromotionId,int _BooksTakenCount)
        {
            //Id = id;
            UserId = _UserId;
            PromotionId = _PromotionId;
            BooksTakenCount = _BooksTakenCount;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewPromoUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
