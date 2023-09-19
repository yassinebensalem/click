using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Domain.Models;


namespace DDD.Domain.Interfaces
{
  public  interface ILanguageRepository : IRepository<Language>
    {
        Task<IEnumerable<Language>> GetUsedLanguages();

    }
}
