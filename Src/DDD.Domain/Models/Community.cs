using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class Community :EntityAudit
    {
        [Required]
        public string CommunityName { get; set; }
        public bool Status { get; set; }
        public ICollection<JoinRequest> JoinRequests { get; set; }
        public ICollection<Promotion> Promotions { get; set; }
        public List<CommunityMember> Members { get; set; }
        public ICollection<WalletTransaction> WalletTransactions { get; set; }
        [NotMapped]
        public ApplicationUser Admin { get; set; }
        public Community( Guid id, string communityName, bool status)
        {
            Id = id;
            CommunityName = communityName;
            Status = status;
        }

        public Community()
        {
        }
    }

}
