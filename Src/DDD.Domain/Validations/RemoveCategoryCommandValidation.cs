using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
   public class RemoveCategoryCommandValidation : CategoryValidation<RemoveCategoryCommand>
    {
        public RemoveCategoryCommandValidation()
        {
            ValidateId();
        }
    }
}
