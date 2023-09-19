using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class PagedSelledBooks
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public long Count { get; set; }
    }
}
