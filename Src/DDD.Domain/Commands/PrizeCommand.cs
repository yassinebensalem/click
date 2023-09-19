using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace DDD.Domain.Commands
{
   public abstract class PrizeCommand : Command
    {
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
