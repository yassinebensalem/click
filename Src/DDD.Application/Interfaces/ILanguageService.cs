using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
   public interface ILanguageService : IDisposable
    {
        IEnumerable<LanguageViewModel> GetAll();
        LanguageViewModel GetLanguageById(int Id);
        Task<IEnumerable<LanguageViewModel>> GetUsedLanguages();
    }
}
