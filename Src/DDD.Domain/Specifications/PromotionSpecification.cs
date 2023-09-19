using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Specifications
{
    public class PromotionsPaginatedSpecification : BaseSpecification<Promotion>
    {
        public PromotionsPaginatedSpecification(int skip, int take)
            : base(i => true)
        {
            AddInclude(b => b.PromotionBook);
            ApplyPaging(skip, take);
        }
    }

    public class FreePromotionsSpecification : BaseSpecification<Promotion>
    {
        public FreePromotionsSpecification()
            : base(b => true && b.PromotionType == PromotionType.Free && !b.IsDeleted)
        {
            AddInclude(b => b.PromotionBook);
            ApplyOrderBy(b => b.CreatedAt);
        }
    }

    public class PromotionsByDateRangeSpecification : BaseSpecification<Promotion>
    {
        public PromotionsByDateRangeSpecification(DateTime startDate, DateTime endDate)
            : base(b => b.StartDate <= startDate && b.EndDate >= endDate && !b.IsDeleted)
        {
            AddInclude(b => b.PromotionBook);
        }
    }

    public class BooksInPromotionSpecification : BaseSpecification<PromotionBook>
    {
        public BooksInPromotionSpecification(Guid PromotionId)
            : base(b => b.PromotionId == PromotionId && b.Promotion.StartDate <= DateTime.Now
            && b.Promotion.EndDate >= DateTime.Now && !b.IsDeleted)
        {
            AddInclude(b => b.Book);
        }
    }

    public class FreeBooksPromotionSpecification : BaseSpecification<Promotion>
    {
        public FreeBooksPromotionSpecification(int skip, int take)
            : base(b => true && b.PromotionType == PromotionType.Free && b.StartDate <= DateTime.Now && b.EndDate >= DateTime.Now
            && !b.IsDeleted )
        {
            ApplyPaging(skip, take);
            AddInclude(b => b.PromotionBook);
            ApplyOrderBy(b => b.CreatedAt);
        }
    }
     
    public class DiscountBooksPromotionSpecification : BaseSpecification<Promotion>
    {
        public DiscountBooksPromotionSpecification(int skip, int take)
            : base(b => b.PromotionType == PromotionType.Discount && b.StartDate <= DateTime.Now && b.EndDate >= DateTime.Now && !b.IsDeleted)
        {
            ApplyPaging(skip, take);
            AddInclude(b => b.PromotionBook);
            ApplyOrderBy(b => b.CreatedAt);
        }
    }

    public class GetUserIdAndPromoIdSpecification : BaseSpecification<PromoUser>
    {
        public GetUserIdAndPromoIdSpecification(string userId, Guid promoId)
            : base(f => f.PromotionId == promoId && f.UserId == userId && !f.IsDeleted)
        {
        }
    }

    public class GetPromoUserByUserIdSpecification : BaseSpecification<PromoUser>
    {
        public GetPromoUserByUserIdSpecification(string userId)
            : base(f => f.UserId == userId && !f.IsDeleted)
        {
        }
    }
}
