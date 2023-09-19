using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
   public interface IWalletTransactionService : IDisposable

    {
        IEnumerable<WalletTransactionViewModel> GetHistoryByUserId(Guid userId);
        IEnumerable<WalletTransactionViewModel> GetHistoryByUserIdWithPagination(Guid userId, int skip, int take);
        IEnumerable<WalletTransactionViewModel> GetHistoryByCommunityId(Guid communityId);
        IEnumerable<WalletTransactionViewModel> GetHistoryByCommunityIdWithPagination(Guid communityId, int skip, int take);
        double GetUserBalance(Guid userId);
        double GetCommunityBalance(Guid communityId);
        Task<bool> Refill(WalletDispatchTransactionViewModel walletTransaction);
        Task<bool> Withdraw(WalletDispatchTransactionViewModel walletTransaction);
        Task<bool> DispatchToMember(WalletDispatchTransactionViewModel viewModel);
    }
}
