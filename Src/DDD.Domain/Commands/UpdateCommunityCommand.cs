using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
  public  class UpdateCommunityCommand : CommunityCommand
    {
        public UpdateCommunityCommand(Guid id,  string communityName, string adminId, bool status)
        {
            Id = id;
            CommunityName = communityName;
            AdminId = adminId;
            Status =status;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCommunityCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
