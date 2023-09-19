using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class RegisterNewCommunityCommandValidation : CommunityValidation<RegisterNewCommunityCommand>
    {
        public RegisterNewCommunityCommandValidation()
        {
            ValidateCommunityName();
            ValidateStatus();
        }
    }
}
