using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Interfaces
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        List<Book> GetFreeBooks();
        List<Book> GetFreeBooks(int index, int pegeSize);
    }
}
