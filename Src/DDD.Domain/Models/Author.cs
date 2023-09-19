using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class Author : EntityAudit
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Biography { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhotoPath { get; set; }
        public Guid? UserId { get; set; } //Optional

        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        public Author() 
        {
        }

        public Author(Guid id, string firstName, string lastName, string email, string phoneNumber, int? countryId, string biography, DateTime birthdate, string photoPath, Guid? userId)
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
            UserId = userId;
        }
        public Author(string firstName, string lastName, string email, string phoneNumber, string biography, DateTime birthdate, string photoPath)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Biography = biography;
            Birthdate = birthdate;
            PhotoPath = photoPath;

        }
        public Author(Guid _Id, string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Id = _Id;
        }
    }
}
