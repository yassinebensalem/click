using System;

namespace DDD.Domain.Core.Models
{
    public abstract class RelationshipEntityAudit: IEntityAudit
    {
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public override abstract bool Equals(object obj);
        public override abstract int GetHashCode();

        public override abstract string ToString();

        public static bool operator ==(RelationshipEntityAudit a, RelationshipEntityAudit b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(RelationshipEntityAudit a, RelationshipEntityAudit b)
        {
            return !(a == b);
        }
        
    }
}
