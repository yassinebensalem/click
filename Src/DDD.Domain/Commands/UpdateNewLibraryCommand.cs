using System;
using DDD.Domain.Validations;

namespace DDD.Domain.Commands
{
    public class UpdateLibraryCommand : LibraryCommand
    {
        public UpdateLibraryCommand(Guid _Id, string _UserId, Guid _BookId, int _currentPage)
        {
            this.Id = _Id;
            this.UserId = _UserId;
            this.BookId = _BookId;
            this.CurrentPage = _currentPage;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
