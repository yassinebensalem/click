using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class AddNewInvoiceCommandValidation : InvoiceValidation<AddNewInvoiceCommand>
    {
        public AddNewInvoiceCommandValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidateBookId();
            ValidatePrice();
            ValidatePublicationDate();
        }
    }
}
