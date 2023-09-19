using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
   public  class PrizeBookDetailsVM
    {
        public string CurrentEdition { get; set; }
        public List<PrizeBookVM> PrizeBooksList { get; set; }
        public List<PrizeVM> prizeVM { get; set; }
        public List<CategoryViewModel> CategoriesList { get; set; }
        public string CurrentCategory { get; set; }
        public List<AuthorVM> AuthorsList { get; set; }
        //public List<Author> AuthorsList { get; set; }
        public List<BookViewModel> BooksList { get; set; }
        public int currentPageIndex { get; set; }
        public BookViewModel Book { get; set; }
        public List<BookViewModel> RelatedBooks { get; set; }
        public List<BookViewModel> BookListVM { get; set; }

        public PrizeBookVM PrizeBookVM { get; set; }

        //public List<Author> AuthorsList { get; set; }
        //public List<PrizeBookVM> EditionList { get; set; }
        //public int currentPageIndex { get; set; }
        //public int PageCount { get; set; }
        //public string KeyWord { get; set; }
        //public int SearchType { get; set; }

    }
}
