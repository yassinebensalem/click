using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
   public class InvoiceByEditorFilterPaginatedSpecification : BaseSpecification<Invoice>
    {
        public InvoiceByEditorFilterPaginatedSpecification(int skip, int take,string userId)
            : base(i => i.Book.Publisher.Id == userId)
        {
            AddInclude(b => b.Book);
            AddInclude(b => b.User);
            ApplyPaging(skip, take);
        }
    }
}
