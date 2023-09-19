using DDD.Domain.Commands;


namespace DDD.Domain.Validations
{
    public class RemoveFavoriteCategoryCommandValidation : FavoriteCategoryValidation<RemoveFavoriteCategoryCommand>
    {
        public RemoveFavoriteCategoryCommandValidation()
        {
            ValidateId();
        }
    }
}
