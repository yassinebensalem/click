using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;
using System.Xml.Linq;
using DDD.Domain.Validations;
using FluentValidation;

namespace DDD.Domain.Commands
{
    public class UpdatePromoUserCommand : PromoUserCommand
    {
        public UpdatePromoUserCommand( string _userId, Guid _PromotionId, int _BooksTakenCount)
        {
            UserId = _userId;
            PromotionId = _PromotionId;
            BooksTakenCount = _BooksTakenCount;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdatePromoUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
