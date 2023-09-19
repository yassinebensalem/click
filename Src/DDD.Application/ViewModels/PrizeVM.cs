using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DDD.Application.ViewModels
{
   public class PrizeVM
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public string WebSiteUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LogoPath { get; set; }
        public IFormFile Logo { get; set; }

    }
}
