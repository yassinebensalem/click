using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class PromotionBookValidation<T> : AbstractValidator<T> where T : PromotionBookCommand
    {

        protected void ValidateBookId()
        {
            RuleFor(c => c.BookId)
               ;
        }


        protected void ValidatePromotiondId()
        {
            RuleFor(c => c.PromotionId)
               ;
        } 
    }
}
