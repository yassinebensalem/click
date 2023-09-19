using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DDD.Domain.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhotoPath { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public char? Gender { get; set; }
        public double RateOnOriginalPrice { get; set; }
        public double RateOnSale { get; set; }
        public int? BooksNumber { get; set; }
        //public string EditorName { get; set; }
        public string RaisonSocial { get; set; }
        public string IdFiscal { get; set; }
        public bool isActive { get; set; }
        public DateTime? CreatedAt { get; set; }

        //Publisher Books
        public List<Book> Books { get; set; }
        //Subscriber Invoices
        public List<Invoice> Invoices { get; set; }

        public List<CommunityMember> Members { get; set; }
        public ICollection<WalletTransaction> WalletTransactions { get; set; }
    }
}
