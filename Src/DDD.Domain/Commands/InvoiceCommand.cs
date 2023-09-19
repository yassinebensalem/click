using System;
using DDD.Domain.Core.Commands;
using static DDD.Domain.Common.Constants.GlobalConstant;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Commands
{
    public abstract class InvoiceCommand : Command
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public string OrderNumber { get; set; }
        public string AuthorizationCode { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentReason PaymentReason { get; set; }
    }
}
