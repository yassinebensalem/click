using System;

namespace DDD.Application.EventSourcedNormalizers

{
    public class BookHistoryData
    {
        public string Action { get; set; }
        public string When { get; set; }
        public string Who { get; set; }

        public string Id { get; set; }
        public string Title { get; set; }
        public string PageNumbers { get; set; }
        public string CoverPath { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string PublicationDate { get; set; }
        public string ISBN { get; set; }
        public string ISSN { get; set; }
        public string EISBN { get; set; }
        public string PDFPath { get; set; }
        public bool IsFree { get; set; }
        public string AuthorId { get; set; }
        public string PublisherId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CountryId { get; set; }
        public Guid LanguageId { get; set; }
    }
}
