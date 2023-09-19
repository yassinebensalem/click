using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class PagedBestPublishers
    {
        public string PublisherId { get; set; }
        public string RaisonSociale { get; set; }
        public long Count { get; set; }
    }
}
