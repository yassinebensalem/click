using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
  public  class RegisterNewCommunityCommand : CommunityCommand
    {
        public RegisterNewCommunityCommand(Guid id, string communityName, string adminId, bool status)
        {
            Id = id;
            CommunityName = communityName;
            AdminId = adminId;
            Status = status;
        }

        public RegisterNewCommunityCommand(string communityName, string adminId, bool status)
        {
            CommunityName = communityName;
            AdminId = adminId;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewCommunityCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
