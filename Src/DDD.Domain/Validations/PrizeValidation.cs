using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class PrizeValidation<T> : AbstractValidator<T> where T : PrizeCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id);
        }

        protected void ValidateName()
        {
            RuleFor(c => c.Name);
        }

        protected void ValidateDescription()
        {
            RuleFor(c => c.Description);
        }

        protected void ValidateCountryId()
        {
            RuleFor(c => c.CountryId);
        }
        protected void ValidateWebSiteUrl()
        {
            RuleFor(c => c.WebSiteUrl);
        }

        protected void ValidateFacebookUrl()
        {
            RuleFor(c => c.FacebookUrl);
        }
    }
}
