using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DDD.Domain.Models;

namespace DDD.Domain.Interfaces
{
    public interface ICommunityRepository : IRepository<Community>
    {
        Community GetById(Guid id, bool withDetails = true);
        IQueryable<Community> GetAllWithMembers(Expression<Func<Community, bool>> predicate = null, bool includeIsDeleted = false);
        bool IsMember(Guid? userId, Guid? communityId);
        bool CheckMembers(IEnumerable<string> userIds, Guid? communityId);
    }
}
