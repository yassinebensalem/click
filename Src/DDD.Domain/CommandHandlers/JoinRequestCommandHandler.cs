using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Events;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using MediatR;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Domain.CommandHandlers
{
    public class JoinRequestCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewJoinRequestCommand, bool>,
        IRequestHandler<UpdateJoinRequestCommand, bool>,
        IRequestHandler<RemoveJoinRequestCommand, bool>

    {

        private readonly IJoinRequestRepository _JoinRequestRepository;
        private readonly IMediatorHandler Bus;


        public JoinRequestCommandHandler(IJoinRequestRepository JoinRequestRepository,
                                     IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _JoinRequestRepository = JoinRequestRepository;
           Bus = bus;
        }

        

    public Task<bool> Handle(RegisterNewJoinRequestCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var joinRequest = new JoinRequest(Guid.NewGuid(), message.FirstName, message.LastName, message.Email, message.Description, message.PhoneNumber, message.CountryId, message.RequesterType, message.Status, message.RaisonSocial,message.IdFiscal, message.VoucherNumber,message.VoucherValue, JsonSerializer.Serialize(message.DesiredBooks),message.ReceiverEmail) ;
            _JoinRequestRepository.Add(joinRequest);

            if (Commit())
            {
                Bus.RaiseEvent(new JoinRequestRegisteredEvent(joinRequest.Id, joinRequest.FirstName, joinRequest.LastName, joinRequest.Email, joinRequest.Description, joinRequest.PhoneNumber , joinRequest.CountryId, joinRequest.RequesterType,
                    joinRequest.Status, joinRequest.RaisonSocial, joinRequest.IdFiscal, message.VoucherNumber, message.VoucherValue, JsonSerializer.Serialize(message.DesiredBooks), message.ReceiverEmail));
            }
            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateJoinRequestCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var joinRequest = new JoinRequest(message.Id, message.FirstName, message.LastName, message.Email, message.Description, message.PhoneNumber, message.CountryId, message.RequesterType, message.Status, message.RaisonSocial, message.IdFiscal,message.VoucherNumber, message.VoucherValue, JsonSerializer.Serialize(message.DesiredBooks), message.ReceiverEmail);

            var existingJoinRequest = _JoinRequestRepository.GetById(joinRequest.Id);

            existingJoinRequest.FirstName = message.FirstName;
            existingJoinRequest.LastName = message.LastName;
            existingJoinRequest.Email = message.Email;
            existingJoinRequest.Description = message.Description;
            existingJoinRequest.PhoneNumber = message.PhoneNumber;
            existingJoinRequest.CountryId = message.CountryId;
            existingJoinRequest.RequesterType = message.RequesterType;
            existingJoinRequest.Status = message.Status;
            existingJoinRequest.RaisonSocial = message.RaisonSocial;
            existingJoinRequest.IdFiscal = message.IdFiscal;
            existingJoinRequest.VoucherNumber = message.VoucherNumber;
            existingJoinRequest.VoucherValue = message.VoucherValue;
            existingJoinRequest.DesiredBooks = JsonSerializer.Serialize(message.DesiredBooks);
            existingJoinRequest.ReceiverEmail = message.ReceiverEmail;

            if (existingJoinRequest != null && existingJoinRequest.Id != joinRequest.Id)
            {
                if (!existingJoinRequest.Equals(joinRequest))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The .... is already existing."));
                    return Task.FromResult(false);
                }
            }

            _JoinRequestRepository.Update(existingJoinRequest);

            if (Commit())
            {
                Bus.RaiseEvent(new JoinRequestUpdatedEvent(joinRequest.Id, joinRequest.FirstName, joinRequest.LastName, joinRequest.Email, joinRequest.Description, joinRequest.PhoneNumber, joinRequest.CountryId, joinRequest.RequesterType,
                    joinRequest.Status));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveJoinRequestCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }
             
            _JoinRequestRepository.Remove(message.Id);
            if (Commit())
            {
                Bus.RaiseEvent(new JoinRequestRemovedEvent(message.Id));
            }

            return Task.FromResult(false);

        }


        public void Dispose()
        {
            _JoinRequestRepository.Dispose();
        }
    }

      


}
    
      
   

