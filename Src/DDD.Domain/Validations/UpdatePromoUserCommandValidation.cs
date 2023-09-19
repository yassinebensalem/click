using DDD.Domain.Commands;
 
namespace DDD.Domain.Validations
{
    public class UpdatePromoUserCommandValidation : PromoUserValidation<UpdatePromoUserCommand>
    {
        public UpdatePromoUserCommandValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidatePromotionId(); 
        }
    }
}
