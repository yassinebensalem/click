using System;

namespace DDD.Domain.Core.Models
{
    public abstract class EntityAudit : Entity, IEntityAudit
    {
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
