using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    public class UpdatePromotionCommandValidation : PromotionValidation<UpdatePromotionCommand>
    {

        public UpdatePromotionCommandValidation()
        {
            ValidateName();
            ValidateDescription();
            ValidateCountryId();
            ValidateStartDate();
            ValidateEndDate();
        }
    }
}
