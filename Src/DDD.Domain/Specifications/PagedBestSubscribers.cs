using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class PagedBestSubscribers
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
        public double Value { get; set; }
    }
}
