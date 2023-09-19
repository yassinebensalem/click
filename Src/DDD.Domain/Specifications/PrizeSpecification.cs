using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class PrizeSpecification
    {
        //    public CategorySpecification(string categoryName, int currentPageIndex, int take)
        //: base(b => b.Category.CategoryName.ToUpper().Contains(categoryName.ToUpper()) && b.Status == Common.Constants.State.BookState.published)
        //    {
        //        AddInclude(b => b.Category);
        //        ApplyPaging(currentPageIndex, take);
        //    }
    }

    public class BookbyPrizeSpecifiacation : BaseSpecification<PrizeBook>
    {
        public BookbyPrizeSpecifiacation(string edition)
           : base(b => b.Edition.Contains(edition))
        {
           // AddInclude(b => b.Book);
            // ApplyPaging(currentPageIndex, take);
            //         (b => b.Title);
        }
    }


}





