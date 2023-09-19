using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public class DissociateCommunityMemberCommandValidation : CommunityMemberCommandValidation<CommunityMemberCommand>
    {
        public DissociateCommunityMemberCommandValidation() : base() { }
    }
}
