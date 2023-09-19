using DDD.Domain.Commands;


namespace DDD.Domain.Validations
{
    public class RegisterNewPromoUserCommandValidation : PromoUserValidation<RegisterNewPromoUserCommand>
    {
        public RegisterNewPromoUserCommandValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidatePromotionId();
        }
    }
}
