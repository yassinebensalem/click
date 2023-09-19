using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.ViewModels
{
    public class CountryComponentVM
    {
        public int SelectedCountryId { get; set; }
        public IEnumerable<CountryViewModel> Countries { get; set; }
    }
}
