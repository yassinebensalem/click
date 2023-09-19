using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class RegisterNewPromotionBookCommandValidation : PromotionBookValidation<RegisterNewPromotionBookCommand>
    {

        public RegisterNewPromotionBookCommandValidation()
        {
            ValidateBookId();
            ValidatePromotiondId(); 
        }

    }
}
