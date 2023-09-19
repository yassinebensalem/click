using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;
using Microsoft.AspNetCore.Http;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Application.ViewModels
{
    public class PromotionVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; } //  Free = 0, Discount = 1
        public int? Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int? CountryId { get; set; } 
        public IFormFile Image { get; set; }
        public bool IsDeleted { get; set; }
        public int MaxFreeBooks { get; set; }
        public ICollection<PromotionBook> PromotionBook { get; set; }
    }
}
