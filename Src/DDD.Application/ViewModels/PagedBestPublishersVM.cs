using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class PagedBestPublishersVM
    {
        public string PublisherId { get; set; }
        public string RaisonSociale { get; set; }
        public long Count { get; set; }
    }
}
