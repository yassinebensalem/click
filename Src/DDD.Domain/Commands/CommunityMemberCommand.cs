using System;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
    public abstract class CommunityMemberCommand : Command
    {
        public Guid CommunityId { get; set; }
        public string MemberId { get; set; }
        public bool IsCommunityAdmin { get; set; }
        public bool Status { get; set; }

    }
}
