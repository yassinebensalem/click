using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Application.ViewModels
{
    public class BookViewModel
    {
        [Key]
        public Guid Id { get; set; }

        // [Required(ErrorMessage = "Title is Required")]
        [MinLength(2)]
        [MaxLength(100)]
        [DisplayName("Book Title")]
        public string Title { get; set; }
        //public string edition { get; set; }

        // [Required(ErrorMessage = "The Cover is Required")]
        [DisplayName("Book Cover")]
        public string CoverPath { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; }

        public double BusinessPrice { get; set; }

        // [Required(ErrorMessage = "Description is Required")]
        [MinLength(2)]
        [DisplayName("Description")]
        public string Description { get; set; }

        // [Required(ErrorMessage = " Page Numbers is Required")]
        [DisplayName(" Page Numbers")]
        public int PageNumbers { get; set; }

        // [Required(ErrorMessage = "The Publication Date is Required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Date in invalid format")]
        [DisplayName("Release Date")]
        public DateTime PublicationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime statusUpdateDate { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [DisplayName(" ISBN")]
        public string ISBN { get; set; }

        [DisplayName("ISSN")]
        public string ISSN { get; set; }

        [DisplayName("EISBN")]
        public string EISBN { get; set; }

        // [Required(ErrorMessage = "The PDF File is Required")]
        [MinLength(2)]
        [DisplayName("Book PDF")]
        public string PDFPath { get; set; }
        public Guid AuthorId { get; set; }
        public string PublisherId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public int CountryId { get; set; }
        public int LanguageId { get; set; }

        [DisplayName("Book PDF")]
        public IFormFile PDFFile { get; set; }

        [DisplayName("Book Cover")]
        public IFormFile CoverFile { get; set; }
        public string CountryName { get; set; }
        public string LanguageName { get; set; }
        public bool isFavorite { get; set; }
        public bool IsDeleted { get; set; }
        public bool inCart { get; set; }
        public bool inLibrary { get; set; }
        public bool CanBeAddedToLib { get; set; }
        public string LibraryId { get; set; }

        public int CurrentPage { get; set; }
        public BookState Status { get; set; }
        public List<BookViewModel> BookListVM { get; set; }

        public PrizeBookVM PrizeBookVM { get; set; }
        //public List<PrizeBookVM> PrizeBooksList { get; set; }
        public string AwardEdition { get; set; }
        public int PromotionsPercentage { get; set; }
        public Guid PromotionId { get; set; }

    }
}
