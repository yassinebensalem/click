using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class SingleEditorDetailsVM
    {
        public ApplicationUser Editor { get; set; }
        public List<BookViewModel> PublishedBooks { get; set; }
        public int PublishedBooksCount { get; set; }
    }
}
