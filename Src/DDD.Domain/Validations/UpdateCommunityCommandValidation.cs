using DDD.Domain.Commands;
namespace DDD.Domain.Validations
{
public    class UpdateCommunityCommandValidation : CommunityValidation<UpdateCommunityCommand>
    {
        public UpdateCommunityCommandValidation()
        {
            ValidateId();
            ValidateCommunityName();
            ValidateStatus();
           
        }
    }
}
