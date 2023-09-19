using System;
using DDD.Domain.Core.Commands;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Commands
{

    public abstract class BookCommand : Command
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int PageNumbers { get; set; }
        public string CoverPath { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ISBN { get; set; }
        public string ISSN { get; set; }
        public string EISBN { get; set; }
        public string PDFPath { get; set; }
        public bool IsFree { get; set; }
        public BookState Status { get; set; }
        public Guid PrizedId { get; set; }
        public string PrizedName { get; set; }

        //R.F
        public Guid AuthorId { get; set; }
        public string PublisherId { get; set; }
        public Guid CategoryId { get; set; }
        public int? CountryId { get; set; }
        public int LanguageId { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
