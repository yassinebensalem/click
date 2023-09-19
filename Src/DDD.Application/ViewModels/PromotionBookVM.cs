using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.ViewModels
{
    public class PromotionBookVM
    {
        public Guid Id { get; set; }
        public Guid PromotionId { get; set; }
        public Guid BookId { get; set; }
    }
}
