using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Events;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Events
{
    public class PromotionUpdatedEvent : Event
    {

        public PromotionUpdatedEvent(Guid _Id, string name, PromotionType promotionType, int? value, DateTime startPublicationDate, DateTime endDate, string description, string imagePath, int? countryId)
        {
            Id = _Id;
            Name = name;
            PromotionType = promotionType;
            Percentage = value;
            StartDate = startPublicationDate;
            EndDate = endDate;
            Description = description;
            ImagePath = imagePath;
            CountryId = countryId;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; }
        public int? Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int? CountryId { get; set; }
    }
}
