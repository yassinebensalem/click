using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class CommunityMember : RelationshipEntityAudit
    {
        public Guid CommunityId { get; set; }
        [ForeignKey("CommunityId")]
        public Community Community { get; set; }
        public string MemberId { get; set; }
        [ForeignKey("MemberId")]
        public ApplicationUser Member { get; set; }
        public bool IsCommunityAdmin { get; set; }
        public bool Status { get; set; }
        
        public CommunityMember( Guid communityId, string memberId, bool isCommunityAdmin, bool status)
        {
            CommunityId = communityId;
            MemberId = memberId;
            IsCommunityAdmin = isCommunityAdmin;
            Status = status;
        }

        public CommunityMember()
        {
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as CommunityMember;

            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;

            return CommunityId.Equals(compareTo.CommunityId) && MemberId.Equals(compareTo.MemberId);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + CommunityId.GetHashCode() + MemberId.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + $" [CommunityId={CommunityId}, MemberId={MemberId}]";
        }

    }

}
