using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;
using FluentValidation;

namespace DDD.Domain.Commands
{
    public class UpdateAuthorCommand : AuthorCommand
    {
        public UpdateAuthorCommand(Guid _Id, string firstname, string lastname, string email, string phonenumber, int _CountryId, string biography, DateTime birthdate, string photopath, Guid userid)
        {
            Id = _Id;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            PhoneNumber = phonenumber;
            CountryId = _CountryId;
            Biography = biography;
            Birthdate = birthdate;
            PhotoPath = photopath;
            UserId = userid;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateAuthorCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
