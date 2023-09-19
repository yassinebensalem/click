using System;
using DDD.Domain.Core.Commands;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class AssociateCommunityMemberCommand : CommunityMemberCommand
    {
        public AssociateCommunityMemberCommand(Guid communityId, string memberId, bool status)
        {
            CommunityId = communityId;
            MemberId = memberId;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = new AssociateCommunityMemberCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
