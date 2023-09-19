using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class RemoveCartCommandValidation : CartValidation<RemoveCartCommand>
    {
        public RemoveCartCommandValidation()
        {
            ValidateId();
        }
    }
}
