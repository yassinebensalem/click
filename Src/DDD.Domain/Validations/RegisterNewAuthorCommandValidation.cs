using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Commands;

namespace DDD.Domain.Validations
{
    class RegisterNewAuthorCommandValidation : AuthorValidation<RegisterNewAuthorCommand>
    {
        public RegisterNewAuthorCommandValidation()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidateEmail();
           //ValidatePhoneNumber();
           //ValidateBiography();
          // ValidateBirthdate();
            //ValidatePhotoPath();
            //ValidateUserId();
            
        }
    }
}
