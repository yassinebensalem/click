using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Events;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Models.Enums;
using DDD.Domain.Specifications;
using DDD.Infra.Data.Repository;
using DDD.Infra.Data.Repository.EventSourcing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace DDD.Application.Services
{
    public class WalletTransactionService : IWalletTransactionService
    {
        private readonly IMapper _mapper;
        private readonly IWalletTransactionRepository _walletRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommunityService _communityService;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public WalletTransactionService(IMapper mapper, IWalletTransactionRepository walletRepository,
            UserManager<ApplicationUser> userManager,
            ICommunityService communityService,
            IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _walletRepository = walletRepository;
            _communityService = communityService;
            _userManager = userManager;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }

        //start implementing needed services
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        
        public IEnumerable<WalletTransactionViewModel> GetHistoryByUserId(Guid userId)
        {
            return _walletRepository.GetAll(x => x.Status && x.ApplicationUserId == userId.ToString()).ProjectTo<WalletTransactionViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<WalletTransactionViewModel> GetHistoryByUserIdWithPagination(Guid userId, int skip, int take)
        {
            return _walletRepository.GetAll(new WalletTransactionFilterPaginatedSpecification(skip, take),
                x => x.Status && x.ApplicationUserId == userId.ToString()).ProjectTo<WalletTransactionViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<WalletTransactionViewModel> GetHistoryByCommunityId(Guid communityId)
        {
            return _walletRepository.GetAll(x => x.Status && string.IsNullOrEmpty(x.ApplicationUserId) && x.CommunityId == communityId).ProjectTo<WalletTransactionViewModel>(_mapper.ConfigurationProvider);
        }
                
        public IEnumerable<WalletTransactionViewModel> GetHistoryByCommunityIdWithPagination(Guid communityId, int skip, int take)
        {
            return _walletRepository.GetAll(new WalletTransactionFilterPaginatedSpecification(skip, take),
                x => x.Status && string.IsNullOrEmpty(x.ApplicationUserId) && x.CommunityId == communityId).ProjectTo<WalletTransactionViewModel>(_mapper.ConfigurationProvider);
        }

        public double GetCommunityBalance(Guid communityId)
        {
            var result = _walletRepository.GetAll(x => x.Status && string.IsNullOrEmpty(x.ApplicationUserId) && x.CommunityId == communityId && x.Type == (int)WalletTransactionTypeEnum.Refill)
                .Sum(x => x.Amount)
                - _walletRepository.GetAll(x => x.Status && string.IsNullOrEmpty(x.ApplicationUserId) && x.CommunityId == communityId && x.Type == (int)WalletTransactionTypeEnum.Withdraw)
                .Sum(x => x.Amount);
            return Math.Round(result, 3);
        }

        public double GetUserBalance(Guid userId)
        {
            var result = _walletRepository.GetAll(x => x.Status && x.ApplicationUserId == userId.ToString() && x.Type == (int)WalletTransactionTypeEnum.Refill)
                .Sum(x=>x.Amount)
                - _walletRepository.GetAll(x => x.Status && x.ApplicationUserId == userId.ToString() && x.Type == (int)WalletTransactionTypeEnum.Withdraw)
                .Sum(x => x.Amount);
            return Math.Round(result, 3);
        }

        public async Task<bool> Refill(WalletDispatchTransactionViewModel viewModel)
        {
            try
            {
                var checkParams = CheckCommunityAndUsers(viewModel);
                if (!checkParams)
                {
                    return false;
                }

                var refillCommand = _mapper.Map<RefillWalletTransactionCommand>(viewModel);
                await Bus.SendCommand(refillCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public async Task<bool> Withdraw(WalletDispatchTransactionViewModel viewModel)
        {
            try
            {
                var checkParams = CheckCommunityAndUsers(viewModel);
                if (!checkParams)
                {
                    return false;
                }

                var refillCommand = _mapper.Map<WithdrawWalletTransactionCommand>(viewModel);
                await Bus.SendCommand(refillCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public async Task<bool> DispatchToMember(WalletDispatchTransactionViewModel viewModel)
        {
            var checkParams = CheckCommunityAndUsers(viewModel);
            if (!checkParams)
            {
                return false;
            }

            var walletToRefill = new WalletDispatchTransactionViewModel
            {
                Amount = viewModel.Amount,
                UserIds = viewModel.UserIds,
                Status = true,
                Type = WalletTransactionTypeEnum.Refill
            };

            var walletToWithdraw = new WalletDispatchTransactionViewModel
            {
                Amount = viewModel.Amount,
                CommunityId = viewModel.CommunityId,
                Status = true,
                Type = WalletTransactionTypeEnum.Withdraw
            };

            var balance = GetCommunityBalance(viewModel.CommunityId.Value);
            if (balance < viewModel.Amount)
            {
                return false;
            }

            bool withdrawResult = await Withdraw(walletToWithdraw);
            if (!withdrawResult)
            {
                return false;
            }
            bool refillResult = await Refill(walletToRefill);
            if (!refillResult)
            {
                return false;
            }

            return true;
        }

        private bool CheckCommunityAndUsers(WalletDispatchTransactionViewModel viewModel)
        {
            if (viewModel == null || ((viewModel.UserIds == null || !viewModel.UserIds.Any()) && !viewModel.CommunityId.HasValue))
            {
                return false;
            }
            if (viewModel.CommunityId.HasValue)
            {
                var community = _communityService.GetCommunityById(viewModel.CommunityId.Value);
                if (community == null)
                {
                    return false;
                }
            }
            
            if (viewModel.Type == WalletTransactionTypeEnum.Refill && viewModel.CommunityId.HasValue && !_communityService.CheckMembers(viewModel.UserIds, viewModel.CommunityId))
            {
                return false;
            }

            return true;
        }

    }
}
