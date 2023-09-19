using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RegisterNewPrizeBookCommand : PrizeBookCommand
    {
        public RegisterNewPrizeBookCommand(Guid id, Guid prizeId, Guid bookId, string edition)
        {
            Id = id;
            PrizeId = prizeId;
            BookId = bookId;
            Edition = edition;

        }



        public override bool IsValid()
        {
            ValidationResult = new RegisterNewPrizeBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
