using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using DDD.Domain.Validations;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Commands
{
    public class UpdateBookStateCommand : BookCommand
    {
        public UpdateBookStateCommand(Guid id, BookState Status)
        {
            Id = id;
            this.Status = Status;

        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateBookStateCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
