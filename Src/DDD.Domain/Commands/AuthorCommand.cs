using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Commands;

namespace DDD.Domain.Commands
{
 public abstract class AuthorCommand : Command
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? CountryId { get; set; }
        public string Biography { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhotoPath { get; set; }       
        public Guid? UserId { get; set; } //Optional 
    }
}
