using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RegisterNewPromotionBookCommand : PromotionBookCommand
    {
        public RegisterNewPromotionBookCommand(Guid id, Guid promotionId, Guid bookId)
        {
            Id = id;
            PromotionId = promotionId;
            BookId = bookId; 
        }



        public override bool IsValid()
        {
            ValidationResult = new RegisterNewPromotionBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
