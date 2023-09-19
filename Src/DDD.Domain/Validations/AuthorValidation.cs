using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;
using FluentValidation;

namespace DDD.Domain.Validations
{
    public abstract class AuthorValidation<T> : AbstractValidator<T> where T : AuthorCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        protected void ValidateFirstName()
        {
            RuleFor(c => c.FirstName)
               ;
        }

        protected void ValidateLastName()
        {
            RuleFor(c => c.LastName)
                ;
        }

        protected void ValidateEmail()
        {
            RuleFor(c => c.Email);
                
        }
        protected void ValidatePhoneNumber()
        {
            RuleFor(c => c.PhoneNumber)
               ;
        }
        protected void ValidateBiography()
        {
            RuleFor(c => c.Biography)
              ;
        }
        protected void ValidateBirthdate()
        {
            RuleFor(c => c.Birthdate)
                .NotEmpty()
                .WithMessage("The Publication Date must not be empty");
        }
        protected void ValidatePhotoPath()
        {
            RuleFor(c => c.PhotoPath)
                .NotEmpty().WithMessage("The Description must not be empty");
        }

        protected void ValidateUserId()
        {
            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty);
        }


    }
}
