using System;
using System.Collections.Generic;
using DDD.Domain.Core.Commands;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class WithdrawWalletTransactionCommand : WalletTransactionCommand
    {
        public WithdrawWalletTransactionCommand(ICollection<Guid> userIds, Guid? communityId, float amount, bool status)
        {
            CommunityId = communityId;
            UserIds = userIds;
            Amount = amount;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = new WalletTransactionCommandValidation<WithdrawWalletTransactionCommand>().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
