using DDD.Domain.Commands;
  
namespace DDD.Domain.Validations
{
    public class RegisterNewLibraryCommandValidation : LibraryValidation<RegisterNewLibraryCommand>
    {
        public RegisterNewLibraryCommandValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidateBookId();
        }
    }
}
