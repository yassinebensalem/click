using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Core.Models;
using DDD.Domain.Models.Enums;

namespace DDD.Domain.Models
{
    public class WalletTransaction : EntityAudit
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid? CommunityId { get; set; }
        public Community Community { get; set; }
        public float Amount { get; set; }
        public Guid? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public bool Status { get; set; }
        public int Type { get; set; }

        public WalletTransaction(Guid? userId, Guid? communityId, float amount, bool status, WalletTransactionTypeEnum type, Guid? invoiceId)
        {
            ApplicationUserId = userId?.ToString();
            CommunityId = communityId;
            Amount = amount;
            InvoiceId = invoiceId;
            Status = status;
            Type = (int)type;
        }

        public WalletTransaction()
        {
        }
    }

}
