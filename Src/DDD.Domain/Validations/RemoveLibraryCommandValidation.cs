using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class RemoveLibraryCommandValidation : LibraryValidation<RemoveLibraryCommand>
    {
        public RemoveLibraryCommandValidation()
        {
            ValidateId();
        }
    }
}
