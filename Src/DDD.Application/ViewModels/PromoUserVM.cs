using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.ViewModels
{
    public class PromoUserVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid PromotionId { get; set; }
        public int BooksTakenCount { get; set; }
        public bool isDeleted { get; set;}
    }
}
