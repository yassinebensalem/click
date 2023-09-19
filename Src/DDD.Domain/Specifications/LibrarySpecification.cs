using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{

    public class GetLibrariesByUserIdSpecification : BaseSpecification<Library>
    {
        public GetLibrariesByUserIdSpecification(string userId)
            : base(l => l.UserId == userId)
        {
        }
    }

    public class GetLibraryByUserIdAndBookIdSpecification : BaseSpecification<Library>
    {
        public GetLibraryByUserIdAndBookIdSpecification(string userId, Guid bookId)
            : base(f => f.BookId == bookId && f.UserId == userId && !f.IsDeleted)
        {
        }
    }
}
