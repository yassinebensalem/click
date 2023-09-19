using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.ViewModels;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
public interface IPrizeService : IDisposable
    {
        bool AddPrize(PrizeVM prizeVM);
        bool AddPrizeBook(PrizeBookVM prizeBookVM);
        IEnumerable<PrizeVM> GetAll();
        IEnumerable<PrizeBookVM> GetAllPrizeBook();
        IEnumerable<BookViewModel> GetBookByTitle(PrizePostVM prizePostVM); 
        PrizeVM GetPrizeById(Guid Id);
        IEnumerable<PrizeBookVM> GetBookByPrize(BookByPrize bookByPrize);
        List<PrizeBookVM> ListEdition(string Edition); 


    }
}
