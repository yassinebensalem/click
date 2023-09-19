using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class PagedSelledBooksVM
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }

        public long Count { get; set; }
    }
}
