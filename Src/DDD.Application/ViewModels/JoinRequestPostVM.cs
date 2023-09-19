using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Application.ViewModels
{
     public class JoinRequestPostVM
     {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public JoinRequestType RequesterType { get; set; }
    }
}
