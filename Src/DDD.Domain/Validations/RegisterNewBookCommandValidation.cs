using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class RegisterNewBookCommandValidation : BookValidation<RegisterNewBookCommand>
    {
        public RegisterNewBookCommandValidation()
        {
            ValidateTitle();
            ValidatePublicationDate();
            ValidatePageNumbers();
            //ValidateCoverPath();
            ValidatePrice();
            ValidateISBN();
            //ValidateISSN();
            //ValidateEISBN();
            //ValidatePDFPath();
            ValidateDescription();
        }
    }
}
