using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Common.JSONColumns;
using DDD.Domain.Validations;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Domain.Commands
{
    public class RegisterNewJoinRequestCommand : JoinRequestCommand 
    {
        public  RegisterNewJoinRequestCommand (Guid _Id, string _FirstName , string _LastName , string _Email, string _Description, string _PhoneNumber, int _CountryId, JoinRequestType requesterType, JoinRequestState status, string raisonSocial, string idFiscal, string voucherNumber=null, double? voucherValue=null,string receiverEmail=null)
        {
            Id = _Id;
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
            VoucherNumber = voucherNumber;
            VoucherValue = voucherValue;
            ReceiverEmail = receiverEmail;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewJoinRequestCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
