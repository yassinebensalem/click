using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class RegisterNewCategoryCommandValidation : CategoryValidation<RegisterNewCategoryCommand>
    {
        public RegisterNewCategoryCommandValidation()
        {
            ValidateCategoryName();
            ValidateStatus();
        }
    }
}
