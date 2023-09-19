using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.ViewModels;

namespace DDD.Application.Interfaces
{
    public interface IPromotionService : IDisposable
    {
        IEnumerable<PromotionVM> GetAll();
        IEnumerable<PromotionVM> GetAll(int skip, int take);
        IEnumerable<PromotionBookVM> GetAllPromotionBook();
        IEnumerable<PromotionVM> GetAllFreePromotions();
        bool AddPromotion(PromotionVM promotionVM);
        bool UpdatePromotion(PromotionVM promotionVM);
        bool AddPromotionBook(PromotionBookVM PromotionBookVM);
        bool DeletePromotion(Guid Id);
        PromotionVM GetPromotionById(Guid Id);
        IEnumerable<PromotionVM> GetPromotionsByDateRange(DateTime startDate, DateTime endDate);
        //IEnumerable<PromotionVM> GetDiscountPromotions(DateTime startDate, DateTime endDate);
        //IEnumerable<PromotionVM> GetFreePromotions(DateTime startDate, DateTime endDate);
        List<BookViewModel> GetBooksInPromotion(Guid Id);
        List<BookViewModel> GetFreeBooks(int skip, int take);
        List<BookViewModel> _GetFreeBooks(int skip, int take);
        List<BookViewModel> GetDiscountBooks(int skip, int take);

    }
}
