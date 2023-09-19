using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public class AssociateCommunityMemberCommandValidation : CommunityMemberCommandValidation<CommunityMemberCommand>
    {
        public AssociateCommunityMemberCommandValidation() : base() { }
    }
}
