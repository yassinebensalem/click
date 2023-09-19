using System;
using System.Collections.Generic;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
    public abstract class WalletTransactionCommand : Command
    {
        public Guid? CommunityId { get; set; }
        public ICollection<Guid> UserIds { get; set; }
        public float Amount { get; set; }
        public Guid? InvoiceId { get; set; }
        public bool Status { get; set; }

    }
}
