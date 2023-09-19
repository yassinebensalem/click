using System;
using System.Collections.Generic;
using DDD.Domain.Core.Commands;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RefillWalletTransactionCommand : WalletTransactionCommand
    {
        public RefillWalletTransactionCommand(ICollection<Guid> userIds, Guid? communityId, float amount, bool status, Guid? invoiceId)
        {
            CommunityId = communityId;
            UserIds = userIds;
            Amount = amount;
            InvoiceId = invoiceId;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = new WalletTransactionCommandValidation<RefillWalletTransactionCommand>().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
