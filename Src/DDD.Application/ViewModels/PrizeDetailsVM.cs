using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class PrizeDetailsVM
    { 
        public PrizeVM Prize { get; set; }
        public string CurrentEdition { get; set; }
        public List<string> EditionYears { get; set; }
        public List<CategoryViewModel> EditionCategories { get; set; }
        //public List<Author> EditionAuthors { get; set; }
        public List<AuthorVM> EditionAuthors { get; set; }
        public List<BookViewModel> EditionBooks { get; set; }
        public BookViewModel SingleBook { get; set; } 
    }
}
