using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Common.JSONColumns;
using DDD.Domain.Core.Models;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Domain.Models
{
    public class JoinRequest : EntityAudit
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }

        public int CountryId { get; set; }
        [ForeignKey("CountryId")] 
        public Country Country { get; set; }
        public JoinRequestType RequesterType { get; set; } //1 = Editor, 2 = Author
        public JoinRequestState Status { get; set; } //Accepted = 1, Rejected = 2, Pending = 3
        public string RaisonSocial { get; set; }
        public string IdFiscal { get; set; }
        public string VoucherNumber { get; set; }
        public double? VoucherValue { get; set; }
        public virtual string DesiredBooks { get; set; }
        public string ReceiverEmail { get; set; }
        public Guid? CommunityId { get; set; }
        [ForeignKey("CommunityId")]
        public Community Community { get; set; }

        public JoinRequest(Guid id, string firstName, string lastName, string email, string description, string phoneNumber,int countryId , JoinRequestType requesterType, JoinRequestState status, string raisonSocial, string idFiscal, string voucherNumber=null, double? voucherValue = null, string desiredBooks = null, string receiverEmail = null)
        {
            Id = id;
            FirstName = firstName; 
            LastName = lastName;
            Email = email;
            Description = description;
            PhoneNumber = phoneNumber;
            CountryId = countryId;
            RequesterType = requesterType;
            Status = status;
            RaisonSocial = raisonSocial;
            IdFiscal = idFiscal;
            VoucherNumber = voucherNumber;
            VoucherValue = voucherValue;
            DesiredBooks = desiredBooks;
            ReceiverEmail = receiverEmail;
        }

        public JoinRequest()
        {
        }

    }
}
