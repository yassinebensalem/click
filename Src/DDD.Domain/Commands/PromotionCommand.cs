using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Commands
{
    public abstract class PromotionCommand : Command
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; }  
        public int? Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int? CountryId { get; set; }
        public IFormFile Image { get; set; }


    }
}
