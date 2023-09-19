using DDD.Domain.Commands;


namespace DDD.Domain.Validations
{
    public class RegisterNewFavoriteCategoryCommandValidation : FavoriteCategoryValidation<RegisterNewFavoriteCategoryCommand>
    {
        public RegisterNewFavoriteCategoryCommandValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidateCategoryId();
        }
    }
}
