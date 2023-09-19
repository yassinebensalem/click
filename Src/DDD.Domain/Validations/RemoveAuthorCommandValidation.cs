using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class RemoveAuthorCommandValidation : AuthorValidation<RemoveAuthorCommand>
    {
        public RemoveAuthorCommandValidation()
        {
            ValidateId();
        }
    }
}
