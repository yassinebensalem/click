using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class IndexVM
    {
        public List<BookViewModel> Cat1books { get; set; }
        public List<BookViewModel> Cat2books { get; set; }
        public List<BookViewModel> Books { get; set; }
        public List<BookViewModel> BooksList { get; set; }

        public List<CategoryViewModel> CategoriesList { get; set; }
        public List<ApplicationUser> EditorsList { get; set; }
        public BookViewModel BookViewModel { get; set; }
        //public List<PrizeBookVM> PrizeBooks { get; set; }
         public PrizeBookVM PrizeBookVM { get; set; }

        public List<BookViewModel> BookListArrived { get; set; }
        public List<BookViewModel> BookListSelled { get; set; }
        public List<BookViewModel> BookListPrized { get; set; }
        public List<PromotionVM> PromotionsList { get; set; }

    }
}
