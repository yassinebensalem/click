using DDD.Domain.Commands;


namespace DDD.Domain.Validations
{
    public class RegisterNewCartCommandValidation : CartValidation<RegisterNewCartCommand>
    {
        public RegisterNewCartCommandValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidateBookId();
        }
    }
}
