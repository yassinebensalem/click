using System;
using DDD.Domain.Validations;
using static DDD.Domain.Common.Constants.GlobalConstant;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Commands
{
    public class AddNewInvoiceCommand : InvoiceCommand
    {
        public AddNewInvoiceCommand(Guid _Id, string _UserId, Guid _BookId, DateTime _Date, double _Amount, double _Price,string _orderNumber, PaymentType _paymentType, string _authorizationCode = null, PaymentReason _paymentReason = PaymentReason.Purshase)
        {
            this.Id = _Id;
            this.UserId = _UserId;
            this.BookId = _BookId;
            this.Date = _Date;
            this.Amount = _Amount;
            this.Price = _Price;
            this.PaymentType = _paymentType;
            this.PaymentReason = _paymentReason;
            this.OrderNumber = _orderNumber;
            this.AuthorizationCode = _authorizationCode;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddNewInvoiceCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
