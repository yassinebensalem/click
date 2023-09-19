using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Domain.Events
{
  public class JoinRequestRegisteredEvent : Event
    {
        public JoinRequestRegisteredEvent(Guid id, string firstName, string lastName, string email, string description, string phoneNumber, int _CountryId, JoinRequestType requesterType, JoinRequestState status, string raisonSocial, string idFiscal, string voucherNumber = null, double? voucherValue = null, string desiredBooks = null, string receiverEmail = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Description = description;
            PhoneNumber = phoneNumber;
            CountryId = _CountryId;
            RequesterType = requesterType;
            Status = status;
            RaisonSocial = raisonSocial;
            IdFiscal = idFiscal;
            VoucherNumber = voucherNumber;
            VoucherValue = voucherValue;
            DesiredBooks = desiredBooks;
            ReceiverEmail = receiverEmail;
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryId { get; set; }
        public JoinRequestType RequesterType { get; set; } 
        public JoinRequestState Status { get; set; }
        public string RaisonSocial { get; set; }
        public string IdFiscal { get; set; }
        public string VoucherNumber { get; set; }
        public double? VoucherValue { get; set; }
        public virtual string DesiredBooks { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
