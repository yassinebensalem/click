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
using System.IO;
using MediatR;

namespace DDD.Domain.CommandHandlers
{
    public class InvoiceCommandHandler : CommandHandler,
        IRequestHandler<AddNewInvoiceCommand, bool>
    {
        private readonly IInvoiceReposistory _invoiceRepository;
        private readonly IMediatorHandler Bus;

        public InvoiceCommandHandler(IInvoiceReposistory invoiceRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _invoiceRepository = invoiceRepository;
            Bus = bus;
        }

        public Task<bool> Handle(AddNewInvoiceCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var invoice = new Invoice(message.UserId, message.Date, message.BookId, message.Price, message.OrderNumber, message.PaymentType, message.AuthorizationCode, message.PaymentReason);
              
            _invoiceRepository.Add(invoice);

            if (Commit())
            {
                Bus.RaiseEvent(new InvoiceAddedEvent(Guid.NewGuid(), invoice.UserId, invoice.Date, invoice.BookId, invoice.Price));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _invoiceRepository.Dispose();
        }
    }
}
