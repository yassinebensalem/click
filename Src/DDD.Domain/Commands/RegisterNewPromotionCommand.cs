using System;
using DDD.Domain.Validations;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Commands
{
    public class RegisterNewPromotionCommand : PromotionCommand
    { 
        public RegisterNewPromotionCommand(string name, PromotionType promotionType, int? value, DateTime startPublicationDate, DateTime endDate, string description, string imagePath, int? countryId)
        {
            Name = name;
            PromotionType = promotionType;
            Percentage = value;
            StartDate = startPublicationDate;
            EndDate = endDate;
            Description = description;
            ImagePath = imagePath;
            CountryId = countryId; 
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewPromotionCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
