using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
   public class RemoveCommunityCommandValidation : CommunityValidation<RemoveCommunityCommand>
    {
        public RemoveCommunityCommandValidation()
        {
            ValidateId();
        }
    }
}
