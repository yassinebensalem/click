using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
   public  class PrizeBookVM
    {
        public Guid Id { get; set; }
        public Guid PrizeId { get; set; }
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Edition { get; set; }
        public List<PrizeDetailsVM> BookListVM { get; set; }
        public string AuthorName { get; set; }
        public Guid AuthorId { get; set; }
        public List<AuthorVM> AuthorsList { get; set; }
        public double Price { get; set; }
        public bool IsFree { get; set; }
        public bool inCart { get; set; }
        public bool isFavorite { get; set; }
    }
}
