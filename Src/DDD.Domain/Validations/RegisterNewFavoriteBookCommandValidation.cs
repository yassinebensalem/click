using DDD.Domain.Commands;


namespace DDD.Domain.Validations
{
    public class RegisterNewFavoriteBookCommandValidation : FavoriteBookValidation<RegisterNewFavoriteBookCommand>
    {
        public RegisterNewFavoriteBookCommandValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidateBookId();
        }
    }
}
