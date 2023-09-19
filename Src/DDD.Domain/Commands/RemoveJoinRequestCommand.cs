using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class RemoveJoinRequestCommand : JoinRequestCommand
    {
        public RemoveJoinRequestCommand(Guid id)
        {
            Id = this.Id;
            AggregateId = this.Id;

        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveJoinRequestCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
