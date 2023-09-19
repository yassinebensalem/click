using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.ViewModels
{
    public class BookDetailsVM
    {
        public List<BookViewModel> BookListVM { get; set; }
        public BookViewModel Book { get; set; }
        public List<BookViewModel> RelatedBooks { get; set; }
    }
}
