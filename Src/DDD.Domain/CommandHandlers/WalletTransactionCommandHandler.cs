using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Events;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DDD.Domain.CommandHandlers
{
    public class WalletTransactionCommandHandler : CommandHandler,
        IRequestHandler<RefillWalletTransactionCommand, bool>,
        IRequestHandler<WithdrawWalletTransactionCommand, bool>
    {
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly IMediatorHandler Bus;

        public WalletTransactionCommandHandler(IWalletTransactionRepository walletTransactionRepository,
                                            IMediatorHandler bus, IUnitOfWork uow,
                                            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _walletTransactionRepository = walletTransactionRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RefillWalletTransactionCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            try
            {
                if(message.UserIds == null || !message.UserIds.Any()) {
                    var walletTransaction = new WalletTransaction(null, message.CommunityId, message.Amount, message.Status, WalletTransactionTypeEnum.Refill, message.InvoiceId);
                    _walletTransactionRepository.Add(walletTransaction);
                }
                else
                {
                    foreach(var userId in message.UserIds)
                    {
                        var walletTransaction = new WalletTransaction(userId, message.CommunityId, message.Amount, message.Status, WalletTransactionTypeEnum.Refill, message.InvoiceId);
                        _walletTransactionRepository.Add(walletTransaction);
                    }                   
                }                
            }
            catch
            {
                return Task.FromResult(false);
            }

            if (Commit())
            {
                try
                {
                    Bus.RaiseEvent(new WalletTransactionRefilledEvent(message.UserIds, message.CommunityId,
                                    message.Amount, message.Status));
                }
                catch (Exception ex)
                {

                }                
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(WithdrawWalletTransactionCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            try
            {
                if (message.UserIds == null || !message.UserIds.Any())
                {
                    var walletTransaction = new WalletTransaction(null, message.CommunityId, message.Amount, message.Status, WalletTransactionTypeEnum.Withdraw, message.InvoiceId);
                    _walletTransactionRepository.Add(walletTransaction);
                }
                else
                {
                    foreach (var userId in message.UserIds)
                    {
                        var walletTransaction = new WalletTransaction(userId, message.CommunityId, message.Amount, message.Status, WalletTransactionTypeEnum.Withdraw, message.InvoiceId);
                        _walletTransactionRepository.Add(walletTransaction);
                    }
                }
            }
            catch
            {
                return Task.FromResult(false);
            }

            if (Commit())
            {
                try
                {
                    Bus.RaiseEvent(new WalletTransactionWithdrawnEvent(message.UserIds, message.CommunityId,
                                        message.Amount, message.Status));
                } catch(Exception ex)
                {

                }
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _walletTransactionRepository.Dispose();
        }
    }
}
