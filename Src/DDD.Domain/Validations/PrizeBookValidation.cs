using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
   public abstract class PrizeBookValidation<T> : AbstractValidator<T> where T : PrizeBookCommand
    {

        protected void ValidateBookId()
        {
            RuleFor(c => c.BookId)
               ;
        }


        protected void ValidatePrizedId()
        {
            RuleFor(c => c.PrizeId)
               ;
        }

        protected void ValidateEdition()
        {
            RuleFor(c => c.Edition)
               ;
        }

    }
}
