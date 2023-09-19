using System;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
    public abstract class CommunityCommand : Command
    {
        public Guid Id { get; set; }
        public string CommunityName { get; set; }
        public string AdminId { get; set; }
        public bool Status { get; set; }
    }
}
