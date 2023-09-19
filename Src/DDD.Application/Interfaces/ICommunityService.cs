using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
   public interface ICommunityService : IDisposable

    {
        IEnumerable<CommunityViewModel> GetAll();
        IEnumerable<CommunityViewModel> GetAll(int skip, int take);
        CommunityViewModel GetCommunityById(Guid Id);
        Task<bool> AddCommunity(CommunityEditViewModel communityViewModel);
        Task<bool> UpdateCommunity(CommunityEditViewModel communityViewModel);
        bool DeleteCommunity(Guid id);
        IList<CommunityHistoryData> GetAllHistory(Guid id);
        List<CommunityViewModel> ListCommunity(string CommunityName);
        List<ApplicationUserViewModel> GetUsers(string keywords, int skip, int take);
        List<ApplicationUserViewModel> GetMembers(Guid communityId, string keywords, int skip, int take);
        bool AssociateMember(CommunityMemberViewModel communityMemberViewModel);
        bool DissociateMember(CommunityMemberViewModel communityMemberViewModel);
        Task<bool> InviteWithMembership(CommunityInvitationalViewModel viewModel);
        bool IsMember(Guid? userId, Guid? communityId);
        bool CheckMembers(ICollection<Guid> userIds, Guid? communityId);
    }
}
