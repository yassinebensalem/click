using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DDD.Application.Enum.Constants;

namespace DDD.Application.ViewModels
{
    public class SearchVM
    {
        public string KeyWord { get; set; }
        public int SearchType { get; set; }
        public int currentPageIndex { get; set; }
    }
}
