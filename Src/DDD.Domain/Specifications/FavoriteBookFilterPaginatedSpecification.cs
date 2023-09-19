using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class FavoriteBookFilterPaginatedSpecification : BaseSpecification<FavoriteBook>
    {
        public FavoriteBookFilterPaginatedSpecification(int skip, int take)
            : base(i => true)
        {
            ApplyPaging(skip, take);
        }
    }

    public class GetFavoriteBooksByUserIdSpecification : BaseSpecification<FavoriteBook>
    {
        public GetFavoriteBooksByUserIdSpecification(string userId)
            : base(f => f.UserId == userId)
        {
        }
    }

    public class UserIdAndBookIdSpecification : BaseSpecification<FavoriteBook>
    {
        public UserIdAndBookIdSpecification(string userId, Guid bookId)
            : base(f => f.BookId == bookId
            && f.UserId == userId
            && !f.Book.IsDeleted
            && !f.IsDeleted)
        {
            AddInclude(f => f.Book);
        }
    }
}
