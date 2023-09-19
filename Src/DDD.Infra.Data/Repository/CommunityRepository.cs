using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DDD.Infra.Data.Repository
{
    public class CommunityRepository : Repository<Community>, ICommunityRepository
    {
        ApplicationDbContext _dbContext;
        public CommunityRepository(ApplicationDbContext context)
            : base(context)
        {
            _dbContext = context;
        }

        public override IQueryable<Community> GetAll(ISpecification<Community> spec, Expression<Func<Community, bool>> predicate = null, bool includeIsDeleted = false)
        {
            var communities = base.GetAll(spec, predicate, includeIsDeleted);
            var query = from community in communities
                        join cm in _dbContext.CommunityMembers on community.Id equals cm.CommunityId into leftCm
                        from subCm in leftCm.DefaultIfEmpty()
                        join admin in _dbContext.Users on subCm.MemberId equals admin.Id into leftAdmin
                        from subAdmin in leftAdmin.DefaultIfEmpty()
                        where (subCm == null || (subCm.Status && subCm.IsCommunityAdmin) || !subCm.Status)
                        select new Community
                        {
                            Id = community.Id,
                            CommunityName = community.CommunityName,
                            IsDeleted = community.IsDeleted,
                            CreatedAt = community.CreatedAt,
                            CreatedBy = community.CreatedBy,
                            Status = community.Status,
                            UpdatedAt = community.UpdatedAt,
                            UpdatedBy = community.UpdatedBy,
                            Admin = subAdmin
                        };
            return query;
        }

        public override IQueryable<Community> GetAll(Expression<Func<Community, bool>> predicate = null, bool includeIsDeleted = false)
        {
            var communities = base.GetAll(predicate, includeIsDeleted);
            var query = from community in communities
                        join cm in _dbContext.CommunityMembers on community.Id equals cm.CommunityId into leftCm
                        from subCm in leftCm.DefaultIfEmpty()
                        join admin in _dbContext.Users on subCm.MemberId equals admin.Id into leftAdmin
                        from subAdmin in leftAdmin.DefaultIfEmpty()
                        where (subCm == null || (subCm.Status && subCm.IsCommunityAdmin) || !subCm.Status)
                        select new Community
                        {
                            Id = community.Id,
                            CommunityName = community.CommunityName,
                            IsDeleted = community.IsDeleted,
                            CreatedAt = community.CreatedAt,
                            CreatedBy = community.CreatedBy,
                            Status = community.Status,
                            UpdatedAt = community.UpdatedAt,
                            UpdatedBy = community.UpdatedBy,
                            Admin = subAdmin
                        };
            return query;
        }

        public IQueryable<Community> GetAllWithMembers(Expression<Func<Community, bool>> predicate = null, bool includeIsDeleted = false)
        {
            return base.GetAll(predicate, includeIsDeleted).Include("Members").Include("Members.Member");
        }

        public Community GetById(Guid id, bool withDetails = true)
        {
            var result = _dbContext.Communities.Find(id);
            if (!withDetails) {
                return result;
            }
            var query = from community in _dbContext.Communities.AsNoTracking()
                        join cm in _dbContext.CommunityMembers on community.Id equals cm.CommunityId into leftCm
                        from subCm in leftCm.DefaultIfEmpty()
                        join admin in _dbContext.Users on subCm.MemberId equals admin.Id into leftAdmin
                        from subAdmin in leftAdmin.DefaultIfEmpty()
                        where (subCm == null || (subCm.Status && subCm.IsCommunityAdmin) || !subCm.Status)
                        && community.Id == result.Id
                        select new Community
                        {
                            Id = community.Id,
                            CommunityName = community.CommunityName,
                            IsDeleted = community.IsDeleted,
                            CreatedAt = community.CreatedAt,
                            CreatedBy = community.CreatedBy,
                            Status = community.Status,
                            UpdatedAt = community.UpdatedAt,
                            UpdatedBy = community.UpdatedBy,
                            Admin = subAdmin
                        };
            return query.AsNoTracking().FirstOrDefault();
        }

        public bool IsMember(Guid? userId, Guid? communityId)
        {
            return _dbContext.CommunityMembers.Where(x => x.CommunityId == communityId && x.MemberId == userId.ToString() && x.Status && !x.IsDeleted).Any();
        }

        public bool CheckMembers(IEnumerable<string> userIds, Guid? communityId)
        {
            return _dbContext.CommunityMembers.Where(x => x.CommunityId == communityId && userIds.Contains(x.MemberId) && x.Status && !x.IsDeleted).Count() == userIds.Count();
        }
    }
}
