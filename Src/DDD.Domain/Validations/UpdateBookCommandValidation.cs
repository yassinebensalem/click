using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class UpdateBookCommandValidation : BookValidation<UpdateBookCommand>
    {
        public UpdateBookCommandValidation()
        {
            ValidateId();
          ////ValidateTitle();
           // ValidatePrice();
          //  ValidateDescription();
           // ValidatePublicationDate();
            //ValidatePageNumbers();
            //ValidateCoverPath();
            //ValidateISBN();
            //ValidateISSN();
            //ValidateEISBN();
            //ValidatePDFPath();
            
        }
    }
}
