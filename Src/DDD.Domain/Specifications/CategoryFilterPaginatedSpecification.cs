using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class CategoryFilterPaginatedSpecification : BaseSpecification<Category>
    {
        public CategoryFilterPaginatedSpecification(int skip, int take)
            : base(i => true)
        {
            ApplyPaging(skip, take);
        }
    }

    public class UsedCategorySpecification : BaseSpecification<Category>
    {
        public UsedCategorySpecification()
            : base(c => c.Books.Count > 0 && c.Status == true)
        {
            AddInclude(b => b.Books);
        }
    }
}
