using DDD.Domain.Commands;


namespace DDD.Domain.Validations
{
    public class RemoveFavoriteBookCommandValidation : FavoriteBookValidation<RemoveFavoriteBookCommand>
    {
        public RemoveFavoriteBookCommandValidation()
        {
            ValidateId();
        }
    }
}
