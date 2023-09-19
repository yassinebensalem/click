using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class RemoveBookCommandValidation : BookValidation<RemoveBookCommand>
    {
        public RemoveBookCommandValidation()
        {
            ValidateId();
        }
    }
}
