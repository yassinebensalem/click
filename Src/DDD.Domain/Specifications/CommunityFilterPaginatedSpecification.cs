using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class CommunityFilterPaginatedSpecification : BaseSpecification<Community>
    {
        public CommunityFilterPaginatedSpecification(int skip, int take, List<string> includes = null)
            : base(i => true)
        {
            ApplyPaging(skip, take);
            ApplyIncludes(includes);
        }

        public CommunityFilterPaginatedSpecification(List<string> includes = null)
            : base(i => true)
        {
            ApplyIncludes(includes);
        }
    }

}
