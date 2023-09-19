using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.ViewModels
{
    public class LibraryVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public int CurrentPage { get; set; }
        public BookViewModel Book { get; set; }
    }
    public class UpdateLibraryVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public int CurrentPage { get; set; }
    }
}
