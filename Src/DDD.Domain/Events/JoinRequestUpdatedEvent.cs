using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Domain.Events
{
   public class JoinRequestUpdatedEvent : Event
    {
        public JoinRequestUpdatedEvent(Guid id, string firstName, string lastName, string email, string description ,  string phoneNumber, int _CountryId , JoinRequestType requesterType, JoinRequestState status)
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
    }
    
    
}
