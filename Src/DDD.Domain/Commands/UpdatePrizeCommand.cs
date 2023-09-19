using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;
using FluentValidation;

namespace DDD.Domain.Commands
{
    public class UpdatePrizeCommand : PrizeCommand
    {
        public UpdatePrizeCommand(Guid _Id, string _Name, string _Description, int _CountryId, string _WebSiteUrl, string _FacebookUrl, string _LogoPath)
        {
            Id = _Id;
            Name = _Name;
            Description = _Description;
            CountryId = _CountryId;
            WebSiteUrl = _WebSiteUrl;
            FacebookUrl = _FacebookUrl;
            LogoPath = _LogoPath;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdatePrizeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
