using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class Cart : EntityAudit
    { 
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Guid BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }


        public Cart()
        {
        }

        public Cart(Guid id, string userId, Guid bookId )
        {
            Id = id;
            UserId = userId; 
            BookId = bookId;
        }
    }
}
