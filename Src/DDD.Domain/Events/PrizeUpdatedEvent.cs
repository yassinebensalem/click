using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;

namespace DDD.Domain.Events
{
    public class PrizeUpdatedEvent : Event
    {

        public PrizeUpdatedEvent(Guid _Id, string _Name, string _Description, int? _CountryId, string _WebSiteUrl, string _FacebookUrl)
        {
            Id = _Id;
            Name = _Name;
            Description = _Description;
            CountryId = _CountryId;
            WebSiteUrl = _WebSiteUrl;
            FacebookUrl = _FacebookUrl;

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CountryId { get; set; }
        public string WebSiteUrl { get; set; }
        public string FacebookUrl { get; set; }

    }
}
