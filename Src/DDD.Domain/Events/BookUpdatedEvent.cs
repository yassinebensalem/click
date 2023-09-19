using System;
using DDD.Domain.Core.Events;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Events
{
    public class BookUpdatedEvent : Event
    {
        public BookUpdatedEvent(Guid id, string title, int pageNumbers, string coverPath, double price, string description, DateTime publicationDate
            , Guid AuthorId, string PublisherId, Guid CategoryId, int? CountryId, int LanguageId, string ISBN, string ISSN, string EISBN, string PDFPath, BookState Status)
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
            this.CountryId = CountryId;
            this.LanguageId = LanguageId;
            this.ISBN = ISBN;
            this.ISSN = ISSN;
            this.EISBN = EISBN;
            this.PDFPath = PDFPath;
            //this.IsFree = IsFree;
            this.Status = Status;

        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public int PageNumbers { get; set; }
        public string CoverPath { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid AuthorId { get; set; }
        public string PublisherId { get; set; }
        public Guid CategoryId { get; set; }
        public int? CountryId { get; set; }
        public int LanguageId { get; set; }
        public string ISBN { get; set; }
        public string ISSN { get; set; }
        public string EISBN { get; set; }
        public string PDFPath { get; set; }
        public bool IsFree { get; set; }
        public BookState Status { get; set; }
    }
}
