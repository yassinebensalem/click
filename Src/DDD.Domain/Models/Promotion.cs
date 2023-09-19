using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Core.Models;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Models
{
    public class Promotion : EntityAudit
    { 
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; } //  Free = 0, Discount = 1
        public int? Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }
        public ICollection<PromotionBook> PromotionBook { get; set; }
        public int MaxFreeBooks { get; set; }
        public Guid? CommunityId { get; set; }
        [ForeignKey("CommunityId")]
        public Community Community { get; set; }

        public Promotion()
        {
        }

        public Promotion(Guid id,string name, PromotionType promotionType, int? value, DateTime startDate, DateTime endDate, string description, string imagePath, int? countryId)
        {
            Id = id;
            Name = name;
            PromotionType = promotionType;
            Percentage = value;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            ImagePath = imagePath;
            CountryId = countryId; 
        }
    }
}
