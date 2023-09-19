using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class AuthorUpdatedEvent : Event
    {
        public AuthorUpdatedEvent(Guid id, string firstName, string lastName, string email, string phoneNumber, int? countryId, string biography, DateTime birthdate, string photoPath)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            CountryId = countryId;
            Biography = biography;
            Birthdate = birthdate;
            PhotoPath = photoPath;
        }

        public AuthorUpdatedEvent(string firstname, string lastname, string email, string phonenumber, string biography, DateTime birthdate, string photopath, Guid id)
        {
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            PhoneNumber = phonenumber;
            Biography = biography;
            Birthdate = birthdate;
            PhotoPath = photopath;
            //UserId = id;

        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? CountryId { get; set; }
        public string Biography { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhotoPath { get; set; }
    }
}
