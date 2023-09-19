using System;
using DDD.Domain.Validations;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Domain.Commands
{
    public class RegisterNewBookCommand : BookCommand
    {
        public RegisterNewBookCommand(Guid _BookId, string _Title, int _PageNumbers, string _CoverPath, double _Price, string _Description,
            DateTime _PublicationDate, Guid AuthorId, string PublisherId, Guid CategoryId, int? CountryId, int LanguageId,
            string ISBN, string ISSN, string EISBN, string PDFPath, BookState Status)
        {
            Id = _BookId;
            Title = _Title;
            PageNumbers = _PageNumbers;
            CoverPath = _CoverPath;
            Price = _Price;
            Description = _Description;
            PublicationDate = _PublicationDate;
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

        public RegisterNewBookCommand(  string _Title, int _PageNumbers, string _CoverPath, double _Price, string _Description,
           DateTime _PublicationDate, Guid AuthorId, string PublisherId, Guid CategoryId, int? CountryId, int LanguageId,
           string ISBN, string ISSN, string EISBN, string PDFPath, BookState Status)
        { 
            Title = _Title;
            PageNumbers = _PageNumbers;
            CoverPath = _CoverPath;
            Price = _Price;
            Description = _Description;
            PublicationDate = _PublicationDate;
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

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
