using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
   public class Prize : EntityAudit
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
        public string WebSiteUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string Logo { get; set; }

            public Prize (Guid _Id, string _Name, string _Description, int _CountryId, string _WebSiteUrl, string _FacebookUrl,string _logo)
        {
            Id = _Id;
            Name = _Name; 
            Description = _Description;
            CountryId = _CountryId; 
            WebSiteUrl = _WebSiteUrl; 
            FacebookUrl = _FacebookUrl;
            Logo = _logo;
        
        }


        public Prize()
        {

        }
    }
}
