using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class FavoriteCategoryFilterPaginatedSpecification : BaseSpecification<FavoriteCategory>
    {
        public FavoriteCategoryFilterPaginatedSpecification(int skip, int take)
            : base(i => true)
        {
            ApplyPaging(skip, take);
        }
    }
}
