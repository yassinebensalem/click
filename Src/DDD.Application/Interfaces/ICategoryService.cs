using System;
using System.Collections.Generic;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
   public interface ICategoryService : IDisposable

    {
        IEnumerable<CategoryViewModel> GetAll();
        IEnumerable<CategoryViewModel> GetAll(int skip, int take);
        IEnumerable<CategoryViewModel> GetAllUsedCategories();
        CategoryViewModel GetCategoryById(Guid Id);
        bool AddCategory(CategoryViewModel categoryViewModel);
        bool UpdateCategory(CategoryViewModel categoryViewModel);
        bool DeleteCategory(Guid id);
        IList<CategoryHistoryData> GetAllHistory(Guid id);
        List<CategoryViewModel> ListCategory(string CategoryName);
    }
}
