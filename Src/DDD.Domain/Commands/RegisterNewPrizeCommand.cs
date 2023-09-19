using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RegisterNewPrizeCommand : PrizeCommand
    {
        public RegisterNewPrizeCommand(Guid _Id, string _Name, string _Description , int _CountryId, string _WebSiteUrl, string _FacebookUrl , string _logo)
        {
            Id = _Id;
            Name = _Name;
            Description = _Description;
            CountryId = _CountryId;
            WebSiteUrl = _WebSiteUrl;
            FacebookUrl = _FacebookUrl;
            LogoPath = _logo;

        }




        public override bool IsValid()
        {
            ValidationResult = new RegisterNewPrizeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
