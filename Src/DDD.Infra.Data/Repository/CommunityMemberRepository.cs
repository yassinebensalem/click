using System;
using System.Linq;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
namespace DDD.Infra.Data.Repository
{
    public class CommunityMemberRepository : Repository<CommunityMember>, ICommunityMemberRepository
    {
        public CommunityMemberRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public void Remove(CommunityMember item)
        {
            DbSet.Remove(DbSet.Find(item.CommunityId, item.MemberId));
        }
    }
}
