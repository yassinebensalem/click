using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
  public  class RemoveCommunityCommand : CommunityCommand
    {
        public RemoveCommunityCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveCommunityCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
