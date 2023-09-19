using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public class InviteToCommunityWithMembershipCommandValidation : CommunityMemberCommandValidation<CommunityMemberCommand>
    {
        public InviteToCommunityWithMembershipCommandValidation() : base() { }
    }
}
