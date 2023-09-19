using DDD.Domain.Commands;
namespace DDD.Domain.Validations
{
public    class UpdateCategoryCommandValidation : CategoryValidation<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidation()
        {
            ValidateId();
            ValidateCategoryName();
            ValidateStatus();
           
        }
    }
}
