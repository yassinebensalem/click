using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class GetCartsByUserIdSpecification : BaseSpecification<Cart>
    {
        public GetCartsByUserIdSpecification(string userId)
            : base(f => f.UserId == userId)
        {
            AddInclude(c => c.Book);
        }
    }

    public class GetCartByUserIdAndBookIdSpecification : BaseSpecification<Cart>
    {
        public GetCartByUserIdAndBookIdSpecification(string userId, Guid bookId)
            : base(f => f.BookId == bookId && f.UserId == userId)
        {
        }
    }
}
