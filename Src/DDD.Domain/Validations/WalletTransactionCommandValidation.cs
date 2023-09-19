using System;
using System.Linq;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public class WalletTransactionCommandValidation<T> : AbstractValidator<T> where T : WalletTransactionCommand
    {
        public WalletTransactionCommandValidation()
        {
            ValidatUserOrCommunityId();
            ValidateAmount();
            ValidateStatus();
        }

        protected void ValidatUserOrCommunityId()
        {
            When(c=>!c.CommunityId.HasValue, ()=>
            RuleFor(c => c.UserIds)
                .NotNull());
            When(c => c.UserIds == null || !c.UserIds.Any(), () =>
            RuleFor(c => c.CommunityId)
                .NotNull());
        }

        protected void ValidateAmount()
        {
            RuleFor(c => c.Amount).GreaterThan(0);
        }
        protected void ValidateStatus()
        {
            RuleFor(c => c.Status);
        }

    }
}
