using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Events;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using MediatR;

namespace DDD.Domain.CommandHandlers
{
    public class PromotionCommandHandler : CommandHandler,
          IRequestHandler<RegisterNewPromotionCommand, bool>,
           IRequestHandler<UpdatePromotionCommand, bool>,
        IRequestHandler<RemovePromotionCommand, bool>,
         IRequestHandler<RegisterNewPromotionBookCommand, bool>

    {
        private readonly IPromotionRepository _PromotionRepository;
        private readonly IPromotionBookRepository _promotionBookRepository;
        private readonly IMediatorHandler Bus;

        public PromotionCommandHandler(IPromotionRepository PromotionRepository, IPromotionBookRepository promotionBookRepository, IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)

        {
            _PromotionRepository = PromotionRepository;
            _promotionBookRepository = promotionBookRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewPromotionCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }
            message.Id = Guid.NewGuid();
            var promotion = new Promotion(message.Id, message.Name,message.PromotionType, message.Percentage, message.StartDate, message.EndDate, message.Description, message.ImagePath, message.CountryId);


            _PromotionRepository.Add(promotion);

            if (Commit())
            {
                Bus.RaiseEvent(new PromotionRegisteredEvent(message.Id, message.Name, message.PromotionType, message.Percentage, message.StartDate, message.EndDate, message.Description, message.ImagePath, message.CountryId));

            }

            return Task.FromResult(true);
        }

        //

        public Task<bool> Handle(RegisterNewPromotionBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var promotionbook = new PromotionBook(Guid.NewGuid(), message.PromotionId, message.BookId);
            _promotionBookRepository.Add(promotionbook);

            if (Commit())
            {
                Bus.RaiseEvent(new PromotionBookRegisteredEvent(promotionbook.Id, promotionbook.BookId, promotionbook.PromotionId));

            }

            return Task.FromResult(true);

        }






        public Task<bool> Handle(UpdatePromotionCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var promotion = new Promotion(message.Id, message.Name, message.PromotionType, message.Percentage, message.StartDate, message.EndDate, message.Description, message.ImagePath, message.CountryId);

            var existingpromotion = _PromotionRepository.GetById(promotion.Id);
             
            existingpromotion.Name = message.Name;
            existingpromotion.PromotionType = message.PromotionType;
            existingpromotion.Percentage = message.Percentage;
            existingpromotion.StartDate = message.StartDate  ;
            existingpromotion.EndDate = message.EndDate;
            existingpromotion.Description = message.Description;
            existingpromotion.ImagePath = message.ImagePath;
            existingpromotion.CountryId = message.CountryId;

            if (existingpromotion != null && existingpromotion.Id != promotion.Id)
            {
                if (!existingpromotion.Equals(promotion))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The .... is already existing."));
                    return Task.FromResult(false);
                }
            }

            _PromotionRepository.Update(existingpromotion);

            if (Commit())
            {
                Bus.RaiseEvent(new PromotionUpdatedEvent(promotion.Id, promotion.Name, promotion.PromotionType, promotion.Percentage, promotion.StartDate, promotion.EndDate, promotion.Description, promotion.ImagePath, promotion.CountryId));
            }

            return Task.FromResult(true);




        }

        public Task<bool> Handle(RemovePromotionCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _PromotionRepository.Remove(message.Id);
            if (Commit())
            {
                Bus.RaiseEvent(new JoinRequestRemovedEvent(message.Id));
            }

            return Task.FromResult(false);
        }



        public void Dispose()
        {
            _PromotionRepository.Dispose();
        }


    }

}




