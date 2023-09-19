using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Interfaces
{
    public interface ICommunityMemberRepository : IRepository<CommunityMember>
    {
        void Remove(CommunityMember existingCommunityMember);
    }
}
