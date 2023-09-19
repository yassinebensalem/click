using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;
using DDD.Domain.Models.Enums;

namespace DDD.Application.ViewModels
{
    public class WalletTransactionViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUserViewModel User { get; set; }
        public Guid? CommunityId { get; set; }
        public CommunityViewModel Community { get; set; }
        public float Amount { get; set; }
        public Guid? InvoiceId { get; set; }
        public InvoiceVM Invoice { get; set; }
        public bool Status { get; set; }
        public WalletTransactionTypeEnum Type { get; set; }
        [DisplayName("CreatedAt")]
        public string CreatedAt { get; set; }
        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

    }
}
