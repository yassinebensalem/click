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
  public  class CommunityCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewCommunityCommand, bool>,
        IRequestHandler<UpdateCommunityCommand, bool>,
        IRequestHandler<RemoveCommunityCommand, bool>
       
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly ICommunityMemberRepository _memberRepository;
        private readonly IMediatorHandler Bus;

        public CommunityCommandHandler(ICommunityRepository communityRepository,
                                       ICommunityMemberRepository memberRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _communityRepository = communityRepository;
            _memberRepository = memberRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewCommunityCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var community = new Community(Guid.NewGuid(), message.CommunityName, message.Status);

            //if (_bookRepository.GetByEmail(book.Email) != null)
            //{
            //    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The book e-mail has already been taken."));
            //    return Task.FromResult(false);
            //}

            community = _communityRepository.Add(community);
            message.Id = community.Id;

           _memberRepository.Add(new CommunityMember { CommunityId = community.Id, MemberId = message.AdminId, Status = true, IsCommunityAdmin = true });

            if (Commit())
            {
                try
                {
                    Bus.RaiseEvent(new CommunityRegisteredEvent(community.Id, community.CommunityName, community.Status));
                } catch(Exception ex)
                {

                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateCommunityCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var community = _communityRepository.GetById(message.Id, withDetails : false);
            if(community == null)
            {
                message.ValidationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Not found", "Community not found"));
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            community.CommunityName = message.CommunityName;
            community.Status = message.Status;
            community.IsDeleted = false;



            //if (existingCommunity != null && existingCommunity.Id != community.Id)
            //{
            //    if (!existingCommunity.Equals(community))
            //    {
            //        Bus.RaiseEvent(new DomainNotification(message.MessageType, "This community is already existing."));
            //        return Task.FromResult(false);
            //    }
            //}

            _communityRepository.Update(community);

            var communityMember = _memberRepository.GetAll(x => !x.IsDeleted && x.Status && x.CommunityId == community.Id && x.IsCommunityAdmin).FirstOrDefault();
            if (communityMember != null && communityMember.MemberId != message.AdminId)
            {
                _memberRepository.Remove(communityMember);
                _memberRepository.Add(new CommunityMember { CommunityId = community.Id, MemberId = message.AdminId, Status = true, IsCommunityAdmin = true });
            }

            if (Commit())
            {
                try {
                Bus.RaiseEvent(new CommunityUpdatedEvent(community.Id, community.CommunityName,community.Status));
                }
                catch (Exception ex)
                {

                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveCommunityCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var community = _communityRepository.GetById(message.Id);
            if (community == null)
            {
                message.ValidationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Not found", "Community not found"));
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }


            _communityRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new CommunityRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _communityRepository.Dispose();
        }
    }
}
