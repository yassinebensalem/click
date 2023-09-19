using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Common.Constants
{
    public static class State
    {
        public enum BookState
        {
            Created = 1,
          //  Accepted = 2,
            Rejected = 2,
            published=3,
            unpublished = 4
        }

        public enum PromotionType
        {
            Free = 0,
            Discount = 1 
        } 
    }
}
