using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Core.Models;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Models
{

    public class Book : EntityAudit
    {
        public string Title { get; set; }
        public int PageNumbers { get; set; }
        public string CoverPath { get; set; }
        public double Price { get; set; }
        public Guid AuthorId { get; set; }

        
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        public string PublisherId { get; set; }

        [ForeignKey("PublisherId")]
        public ApplicationUser Publisher { get; set; }


        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public string Description { get; set; }

        public int? _CountryId { get; set; }

        [ForeignKey("_CountryId")]
        public Country Country { get; set; }

        public int _LanguageId { get; set; }

        [ForeignKey("_LanguageId")]
        public Language Language { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime StatusUpdateDate { get; set; } 
        public string ISBN { get; set; } 
        public string ISSN { get; set; } 
        public string EISBN { get; set; }
        public string PDFPath { get; set; }
        //public bool IsFree { get; set; }
        public BookState Status { get; set; } //  Created = 1, Accepted = 2, Rejected =3, unpublished = 4
        public IEnumerable<Invoice> Invoices { get; set; } //  Created = 1, Accepted = 2, Rejected =3, unpublished = 4




        public Book(Guid id, string title, int pageNumbers, string coverPath, double price, Guid AuthorId, string PublisherId, Guid CategoryId, int? CountryId, int LanguageId,
            string description/*, string country*/, DateTime publicationDate, string ISBN, string ISSN, string EISBN, string PDFPath , BookState status)
        {
            Id = id;
            Title = title;
            PageNumbers = pageNumbers;
            CoverPath = coverPath;
            Price = price;
            Description = description;
            PublicationDate = publicationDate;
            this.AuthorId = AuthorId;
            this.PublisherId = PublisherId;
            this.CategoryId = CategoryId;
            this._CountryId = CountryId;
            this._LanguageId = LanguageId;
            this.ISBN = ISBN;
            this.ISSN = ISSN;
            this.EISBN = EISBN;
            this.PDFPath = PDFPath;
            //this.IsFree = IsFree;
            Status = status;
        }

        public Book(Guid id, BookState status)
        {
            Id = id;
            Status = status;
        }
        public Book(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
        // Empty constructor for EF
        protected Book() { }
    }
}
