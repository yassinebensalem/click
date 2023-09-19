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
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DDD.Domain.CommandHandlers
{
    public class CommunityMemberCommandHandler : CommandHandler,
        IRequestHandler<AssociateCommunityMemberCommand, bool>,
        IRequestHandler<DissociateCommunityMemberCommand, bool>,
        IRequestHandler<InviteToCommunityWithMembershipCommand, bool>
    {
        private readonly ICommunityMemberRepository _communityMemberRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediatorHandler Bus;

        public CommunityMemberCommandHandler(ICommunityMemberRepository communityMemberRepository,
                                            IMediatorHandler bus, IUnitOfWork uow,
                                            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _communityMemberRepository = communityMemberRepository;
            Bus = bus;
        }

        public Task<bool> Handle(AssociateCommunityMemberCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            try
            {
                var existingCommunityMember = _communityMemberRepository.GetAll(x => x.CommunityId == message.CommunityId
                                                && x.MemberId == message.MemberId, includeIsDeleted : true).FirstOrDefault();
                if (existingCommunityMember != null)
                {
                    existingCommunityMember.IsCommunityAdmin = message.IsCommunityAdmin;
                    existingCommunityMember.Status = message.Status;
                    existingCommunityMember.IsDeleted = false;
                    _communityMemberRepository.Update(existingCommunityMember);
                }
                else
                {
                    var communityMember = new CommunityMember(message.CommunityId, message.MemberId, message.IsCommunityAdmin, message.Status);
                    _communityMemberRepository.Add(communityMember);
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
                    Bus.RaiseEvent(new CommunityMemberDissociatedEvent(message.CommunityId, message.MemberId, message.IsCommunityAdmin, message.Status));
                }
                catch (Exception ex)
                {
                    
                }                
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(DissociateCommunityMemberCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            try
            {
                var existingCommunityMember = _communityMemberRepository.GetAll(x => x.CommunityId == message.CommunityId
                                                 && x.MemberId == message.MemberId).FirstOrDefault();
                if (existingCommunityMember != null)
                {
                    _communityMemberRepository.Remove(existingCommunityMember);
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
                    Bus.RaiseEvent(new CommunityMemberDissociatedEvent(message.CommunityId, message.MemberId, message.IsCommunityAdmin, message.Status));
                } catch(Exception ex)
                {

                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(InviteToCommunityWithMembershipCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            try
            {

                var existingCommunityMember = _communityMemberRepository.GetAll(x => x.CommunityId == message.CommunityId
                                                && x.MemberId == message.MemberId).FirstOrDefault();
                if (existingCommunityMember != null)
                {
                    existingCommunityMember.IsCommunityAdmin = message.IsCommunityAdmin;
                    existingCommunityMember.Status = message.Status;
                    existingCommunityMember.IsDeleted = false;
                    _communityMemberRepository.Update(existingCommunityMember);
                }
                else
                {
                    var communityMember = new CommunityMember(message.CommunityId, message.MemberId, message.IsCommunityAdmin, message.Status);
                    _communityMemberRepository.Add(communityMember);
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
                    Bus.RaiseEvent(new CommunityMemberAssociatedEvent(message.CommunityId, message.MemberId,
                                        message.IsCommunityAdmin, message.Status));
                } catch (Exception ex){

                }
            }

            return Task.FromResult(true);
        }


        public void Dispose()
        {
            _communityMemberRepository.Dispose();
        }
    }
}
