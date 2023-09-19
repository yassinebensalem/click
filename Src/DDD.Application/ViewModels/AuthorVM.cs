using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DDD.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using static DDD.Domain.Common.Constants.State;
using Azure.Storage.Blobs.Models;

namespace DDD.Application.ViewModels
{
  public  class AuthorVM 
    {
        public List<BookViewModel> BooksList { get; set; }
        public PrizeBookVM PrizeBookVM { get; set; }
        public Guid Id { get; set; }
        public string AuthorName => $"{FirstName} {LastName}".Trim();
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryId { get; set; }
        public string BirthdateAsString { get; set; }
        public DateTime Birthdate { get; set; }
        public IFormFile PhotoFile { get; set; }
        public string PhotoPath { get; set; }

        public string bookId { get; set; }
        public string KeyWord { get; set; }
        public int currentPageIndex { get; set; }
        public string Biography { get;  set; }
        public Guid UserId { get;  set; }
        public int BooksNumber { get; set; }
        //public string EditorName { get; set; }
        public string RaisonSocial { get; set; }
    }
}
