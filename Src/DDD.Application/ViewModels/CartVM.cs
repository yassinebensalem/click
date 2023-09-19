using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class CartVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public BookViewModel Book { get; set; }
    }
}
