using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MipLivrelStore.Models;

namespace MipLivrelStore.ViewComponents
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _memoryCache;

        public CategoryViewComponent(ICategoryService categoryService, IMemoryCache memoryCache)
        {
            _categoryService = categoryService;
            _memoryCache = memoryCache;
        }
        public async Task<IViewComponentResult> InvokeAsync(bool isDropDown = true)
        {
            var categories = await GetAllCategories();
         
            return isDropDown ? View("Index", categories) : View("LeftSidebarList", categories);
        }

        private async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            if (!_memoryCache.TryGetValue(Constants.CategoriesCacheKey, out IEnumerable<CategoryViewModel> categories))
            {
                //calling the server
                //var categoryList = _categoryService.GetAll().Where(cat => cat.Status != false && cat.boo).ToList();
                var categoryList = _categoryService.GetAllUsedCategories().ToList();
                foreach (var category in categoryList)
                {
                    Dictionary<string, string> CategoryNames = JsonSerializer.Deserialize<Dictionary<string, string>>(category.CategoryName);
                    var cat = CategoryNames.Where(x => x.Key == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName).ToList()[0].Value;
                    category.CategoryName = cat;
                }
                categories = categoryList;

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };
                //setting cache entries
                _memoryCache.Set(Constants.CategoriesCacheKey, categories, cacheExpiryOptions);
            }
            return categories.OrderBy(x => x.CategoryName);
        } 
    }
}
