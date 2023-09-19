using System;
using DDD.Domain.Validations;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Commands
{
    public class UpdateBookCommand : BookCommand
    {
       

        public UpdateBookCommand(Guid id, string title, int pageNumbers, string coverPath, double price, string description, DateTime publicationDate
            , Guid AuthorId, string PublisherId, Guid CategoryId, int? CountryId, int LanguageId, string ISBN, string ISSN, string EISBN, string PDFPath,
            BookState Status)
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
            //this.IsFree=IsFree;
            this.Status = Status;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
