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
    public class PrizeCommandHandler : CommandHandler,
          IRequestHandler<RegisterNewPrizeCommand, bool>,
           IRequestHandler<UpdatePrizeCommand, bool>,
        IRequestHandler<RemovePrizeCommand, bool>,
         IRequestHandler<RegisterNewPrizeBookCommand, bool>

    {
        private readonly IPrizeRepository _PrizeRepository;
        private readonly IPrizeBookRepository _prizeBookRepository;
        private readonly IMediatorHandler Bus;

        public PrizeCommandHandler(IPrizeRepository PrizeRepository, IPrizeBookRepository prizeBookRepository , IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)

        {
            _PrizeRepository = PrizeRepository;
            _prizeBookRepository = prizeBookRepository;
             Bus = bus;
        }

        public Task<bool> Handle(RegisterNewPrizeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }
            message.Id = Guid.NewGuid();
            var prize = new Prize(message.Id, message.Name, message.Description, message.CountryId, message.WebSiteUrl, message.FacebookUrl, message.LogoPath);


            _PrizeRepository.Add(prize);

            if (Commit())
            {
                Bus.RaiseEvent(new PrizeRegisteredEvent(prize.Id, prize.Name, prize.Description, prize.CountryId, prize.WebSiteUrl, prize.FacebookUrl , prize.Logo));

            }

            return Task.FromResult(true);
        }

        //

        public Task<bool> Handle(RegisterNewPrizeBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var prizebook = new PrizeBook(Guid.NewGuid(),message.PrizeId, message.BookId, message.Edition);
            _prizeBookRepository.Add(prizebook);

            if (Commit())
            {
                Bus.RaiseEvent(new PrizeBookRegisteredEvent(prizebook.Id, prizebook.BookId, prizebook.PrizeId, prizebook.Edition));

            }

            return Task.FromResult(true);

        }






        public Task<bool> Handle(UpdatePrizeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var prize = new Prize(message.Id, message.Name, message.Description, message.CountryId, message.WebSiteUrl, message.FacebookUrl,message.LogoPath);

            var existingprize = _PrizeRepository.GetById(prize.Id);

            existingprize.Name= message.Name;
            existingprize.Description = message.Description;
            existingprize.CountryId = message.CountryId;
            existingprize.Description = message.Description;
            existingprize.WebSiteUrl = message.WebSiteUrl;
            existingprize.CountryId = message.CountryId;
            existingprize.FacebookUrl = message.FacebookUrl;
            existingprize.Logo = message.LogoPath;

            if (existingprize != null && existingprize.Id != prize.Id)
            {
                if (!existingprize.Equals(prize))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The .... is already existing."));
                    return Task.FromResult(false);
                }
            }

            _PrizeRepository.Update(existingprize);

            if (Commit())
            {
                Bus.RaiseEvent(new PrizeUpdatedEvent(prize.Id, prize.Name, prize.Description, prize.CountryId, prize.WebSiteUrl, prize.FacebookUrl));
            }

            return Task.FromResult(true);


            

        }

        public Task<bool> Handle(RemovePrizeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _PrizeRepository.Remove(message.Id);
            if (Commit())
            {
                Bus.RaiseEvent(new JoinRequestRemovedEvent(message.Id));
            }

            return Task.FromResult(false);
        }

        

        public void Dispose()
        {
            _PrizeRepository.Dispose();
        }

        
    }

}

    


