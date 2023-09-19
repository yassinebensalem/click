using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class Library : EntityAudit
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Guid BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public int CurrentPage { get; set; }
 
        public Library()
        {
        }

        public Library(string userId, ApplicationUser user, Guid bookId, Book book)
        {
            UserId = userId;
            User = user;
            BookId = bookId;
            Book = book;
           
        }

        public Library(string userId, Guid bookId, int currentPage=0)
        {
            UserId = userId;
            BookId = bookId;
            CurrentPage = currentPage;
        }
    }
}
