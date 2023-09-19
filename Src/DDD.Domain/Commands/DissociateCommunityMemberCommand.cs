using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class DissociateCommunityMemberCommand : CommunityMemberCommand
    {
        public DissociateCommunityMemberCommand(Guid communityId, string memberId, bool status)
        {
            CommunityId = communityId;
            MemberId = memberId;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = new DissociateCommunityMemberCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
