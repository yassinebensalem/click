using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MipLivrelStore.Models;

namespace MipLivrelStore.ViewComponents
{
    [ViewComponent(Name = "Country")]
    public class CountryViewComponent : ViewComponent
    {

        private readonly ICountryService _countryService;
        private readonly IMemoryCache _memoryCache;

        public CountryViewComponent(ICountryService countryService, IMemoryCache memoryCache)
        {
            _countryService = countryService;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(int CountryId = 0)
        {
            var countries = await GetAllCountries();
            CountryComponentVM countryComponentVM = new CountryComponentVM();
            countryComponentVM.Countries = countries;
            countryComponentVM.SelectedCountryId = CountryId == 0 ? 0 : CountryId;
            return View("Index", countryComponentVM);
        }

        public async Task<IViewComponentResult> InvokeAsyncWithParam(int CountryId)
        {
            var countries = await GetAllCountries();
            CountryComponentVM countryComponentVM = new CountryComponentVM();
            countryComponentVM.Countries = countries;
            countryComponentVM.SelectedCountryId = CountryId;
            return View("Index", countryComponentVM);
        }

        private async Task<IEnumerable<CountryViewModel>> GetAllCountries()
        {
            if (!_memoryCache.TryGetValue(Constants.CountriesCacheKey, out IEnumerable<CountryViewModel> countries))
            {
                //calling the server
                countries = _countryService.GetAll().ToList();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                //setting cache entries
                _memoryCache.Set(Constants.CountriesCacheKey, countries, cacheExpiryOptions);
            }
            return countries;
        }
    }
}
