using System;
using DDD.Domain.Core.Commands;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class InviteToCommunityWithMembershipCommand : CommunityMemberCommand
    {
        public string Email { get; set; }
        public InviteToCommunityWithMembershipCommand(Guid communityId, string email, bool isCommunityAdmin, bool status)
        {
            CommunityId = communityId;
            Email = email;
            IsCommunityAdmin = isCommunityAdmin;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = new InviteToCommunityWithMembershipCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
