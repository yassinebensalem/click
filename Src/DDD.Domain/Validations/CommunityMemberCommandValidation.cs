using System;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class CommunityMemberCommandValidation<T> : AbstractValidator<T> where T : CommunityMemberCommand
    {
        public CommunityMemberCommandValidation()
        {
            ValidateCommunityId();
            ValidateMemberId();
            ValidateIsCommunityAdmin();
            ValidateStatus();
        }
        protected void ValidateCommunityId()
        {
            RuleFor(c => c.CommunityId)
                .NotEqual(Guid.Empty);
        }
        protected void ValidateMemberId()
        {
            RuleFor(c => c.MemberId)
                .NotEqual(string.Empty);
        }

        protected void ValidateIsCommunityAdmin()
        {
            RuleFor(c => c.IsCommunityAdmin);
        }
        protected void ValidateStatus()
        {
            RuleFor(c => c.Status);
        }

    }
}
