using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Domain.Commands
{
    public class UpdateJoinRequestCommand : JoinRequestCommand
    {
        public UpdateJoinRequestCommand(Guid id, string _FirstName, string _LastName, string _Email, string _Description , string _PhoneNumber, int _CountryId , JoinRequestType requesterType, JoinRequestState status, string raisonSocial, string idFiscal)
        {
            Id = id;
            FirstName = _FirstName;
            LastName = _LastName;
            Email = _Email;
            Description = _Description;
            PhoneNumber = _PhoneNumber;
            CountryId = _CountryId;
            RequesterType = requesterType;
            Status = status;
            RaisonSocial = raisonSocial;
            IdFiscal = idFiscal;
        }
       

        public override bool IsValid()
        {
            ValidationResult = new UpdateJoinRequestCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
