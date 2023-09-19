using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.EventSourcedNormalizers
{
   public class LanguageHistoryData
    {
        public string Action { get; set; }
        public string When { get; set; }
        public string Who { get; set; }
        public string LanguageId { get; set; }
        public string Name { get; set; }
        public string CodeAlpha2 { get; set; }
        public string CodeAlpha3 { get; set; }
        public string Code { get; set; }
    }
}
