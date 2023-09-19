using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Application.ViewModels
{
   public class BookStatePutVM
    {
        public Guid Id { get; set; }
        public BookState Status { get; set; }
    }
}
