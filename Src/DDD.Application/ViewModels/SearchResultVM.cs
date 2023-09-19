using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DDD.Application.Enum.Constants;

namespace DDD.Application.ViewModels
{
    public class SearchResultVM
    {
        public int currentPageIndex { get; set; }
        public int PageCount { get; set; }
        public PrizeBookVM PrizeBookVM { get; set; }
        public List<BookViewModel> BooksList { get; set; }
        public IEnumerable<AuthorVM> AuthorsList { get; set; }
        //public IEnumerable<Author> AuthorsList { get; set; }
        public IEnumerable<CategoryViewModel> CategoriesList { get; set; }
        public IEnumerable<LanguageViewModel> LanguagesList { get; set; }
        public string KeyWord { get; set; }
        public int SearchType { get; set; }
    }
}
