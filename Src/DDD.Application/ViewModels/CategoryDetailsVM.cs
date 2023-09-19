using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
  public  class CategoryDetailsVM
    {
        public string CurrentCategory { get; set; }
        public List<BookViewModel> BooksList { get; set; }
        public List<AuthorVM> AuthorsList { get; set; }
        public List<CategoryViewModel> CategoriesList { get; set; }
        public IEnumerable<LanguageViewModel> LanguagesList { get; set; } 
        public int currentPageIndex { get; set; }
        public int PageCount { get; set; }
        public string KeyWord { get; set; }
        public int SearchType { get; set; }

    }
}
