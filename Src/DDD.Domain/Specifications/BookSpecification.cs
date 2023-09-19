using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class BookFilterPaginatedSpecification : BaseSpecification<Book>
    {
        public BookFilterPaginatedSpecification(int skip, int take)
            : base(i => true)
        {
            ApplyPaging(skip, take);
        }
    }

    public class CategorySpecification : BaseSpecification<Book>
    {
        public CategorySpecification(string categoryName, int currentPageIndex, int take)
            : base(b => b.Category.CategoryName.ToUpper().Contains(categoryName.ToUpper())
            && b.Status == Common.Constants.State.BookState.published
            && !b.IsDeleted && b.Category.Books.Count > 0
            && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Category);
            AddInclude(b => b.Author);
            ApplyOrderByDescending(b => b.PublicationDate);
            ApplyPaging(currentPageIndex, take);
        }
    }

    public class TunisianBooksSpecification : BaseSpecification<Book>
    {
        public TunisianBooksSpecification(int currentPageIndex, int take)
            : base(b => b.Country.CodeAlpha2 == "tn" && b.Status == Common.Constants.State.BookState.published && !b.IsDeleted && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Country);
            AddInclude(b => b.Author);
            ApplyPaging(currentPageIndex, take);
        }
    }

    public class ForeignBooksSpecification : BaseSpecification<Book>
    {
        public ForeignBooksSpecification(int currentPageIndex, int take)
            : base(b => b.Country.CodeAlpha2 != "tn" && b.Status == Common.Constants.State.BookState.published && !b.IsDeleted && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Country);
            AddInclude(b => b.Author);
            ApplyPaging(currentPageIndex, take);
        }
    }

    public class BookTitleAndDescriptionSpecification : BaseSpecification<Book>
    {
        public BookTitleAndDescriptionSpecification(string bookTitle, int currentPageIndex, int take)
            : base(b => b.Title.ToUpper().Contains(bookTitle.ToUpper()) && b.Status == Common.Constants.State.BookState.published && !b.IsDeleted && !b.Author.IsDeleted)
        {
            ApplyPaging(currentPageIndex, take);
            AddInclude(b => b.Author);
            //AddInclude(b => b.Title);
        }
    }

    public class DiscountBookSpecification : BaseSpecification<Book>
    {
        public DiscountBookSpecification(int currentPageIndex, int take)
            : base(b => b.Price == 0 && b.Status == Common.Constants.State.BookState.published && !b.IsDeleted && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Author);
            ApplyPaging(currentPageIndex, take);
        }
    }

    public class FreeBookSpecification : BaseSpecification<Book>
    {
        public FreeBookSpecification(int currentPageIndex, int take)
            : base(b => b.Price == 0 && b.Status == Common.Constants.State.BookState.published && !b.IsDeleted && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Author);
            ApplyPaging(currentPageIndex, take);
        }
    }

    public class NewBooksSpecification : BaseSpecification<Book>
    {
        public NewBooksSpecification(int currentPageIndex, int take)
            : base(b => b.Status == Common.Constants.State.BookState.published /*b.StatusUpdateDate >= DateTime.Now.AddDays(-30) && */
            && !b.IsDeleted && !b.Author.IsDeleted)
        {
            ApplyPaging(currentPageIndex, take);
            AddInclude(b => b.Author);
            ApplyOrderByDescending(b => b.PublicationDate);
        }
    }

    public class AuthorSpecification : BaseSpecification<Book>
    {
        public AuthorSpecification(string authorName, int currentPageIndex, int take)
            : base(b => (b.Author.FirstName.Trim().ToUpper().Contains(authorName.ToUpper())
            || b.Author.LastName.Trim().ToUpper().Contains(authorName.ToUpper())
            //|| String.Concat(String.Concat(b.Author.FirstName.ToUpper(), " "), b.Author.LastName.ToUpper()).Contains(authorName.ToUpper())
            || string.Concat(string.Concat(b.Author.FirstName.Trim().ToUpper(), " "), b.Author.LastName.Trim().ToUpper()).Contains(authorName.ToUpper()))
            && b.Status == Common.Constants.State.BookState.published && !b.IsDeleted && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Author);
            ApplyPaging(currentPageIndex, take);
        }
    }
    public class AuthorIdSpecification : BaseSpecification<Book>
    {
        public AuthorIdSpecification(Guid authorId, int currentPageIndex, int take)
            : base(b => b.AuthorId.Equals(authorId) && b.Status == Common.Constants.State.BookState.published && !b.Author.IsDeleted)
        {
            ApplyPaging(currentPageIndex, take);
            AddInclude(b => b.Author);
        }
    }

    public class AuthorIdSpecification_WithoutPagination : BaseSpecification<Book>
    {
        public AuthorIdSpecification_WithoutPagination(Guid authorId)
            : base(b => b.AuthorId.Equals(authorId) && b.Status == Common.Constants.State.BookState.published && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Author);
        }
    }

    public class EditorSpecification : BaseSpecification<Book>
    {
        public EditorSpecification(string editorName, int currentPageIndex, int take)
            : base(b => b.Publisher.RaisonSocial.ToUpper().Contains(editorName.ToUpper())
            && b.Status == Common.Constants.State.BookState.published
            && !b.IsDeleted && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Publisher);
            ApplyPaging(currentPageIndex, take);
        }
    }

    //public class EditorByIdSpecification : BaseSpecification<Book>
    //{
    //    public EditorByIdSpecification(string editorId, int currentPageIndex, int take)
    //        : base(b => (b.Publisher.Id== editorId)  
    //        && b.Status == Common.Constants.State.BookState.published)
    //    {
    //        AddInclude(b => b.Publisher);
    //        ApplyPaging(currentPageIndex, take);
    //    }
    //}

    public class EditorIdSpecification : BaseSpecification<Book>
    {
        public EditorIdSpecification(string editorId, int currentPageIndex, int take)
            : base(b => b.PublisherId.Equals(editorId) && !b.Author.IsDeleted
            && b.Status == Common.Constants.State.BookState.published
            && !b.IsDeleted)
        {
            AddInclude(b => b.Author);
            ApplyPaging(currentPageIndex, take);
        }
    }

    public class RelatedBooksSpecification : BaseSpecification<Book>
    {
        public RelatedBooksSpecification(Guid categoryId, int currentIndex, int take)
        : base(b => b.CategoryId == categoryId && b.Status == Common.Constants.State.BookState.published && !b.Author.IsDeleted)
        {
            ApplyPaging(currentIndex, take);
            AddInclude(b => b.Author);
            ApplyOrderByDescending(b => b.PublicationDate);
        }
    }

    public class BookbyEditeurIntervalSpecification : BaseSpecification<Book>
    {
        public BookbyEditeurIntervalSpecification(DateTime fromDate, DateTime toDate, string publisherId)

          : base(b => b.PublisherId == publisherId
          && b.CreatedAt.Value.Date >= fromDate
          && b.CreatedAt.Value.Date <= toDate
          && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Author);

        }
    }

    public class BookbyIntervalSpecification : BaseSpecification<Book>
    {
        public BookbyIntervalSpecification(DateTime fromDate, DateTime toDate)

          : base(b => fromDate.Date <= b.CreatedAt.Value.Date && b.CreatedAt.Value.Date < toDate.Date && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Author);

        }
    }

    public class BookTitle : BaseSpecification<Book>
    {
        public BookTitle(string bookTitle)
            : base(b => b.Title.Contains(bookTitle) && b.Status != Common.Constants.State.BookState.Rejected && !b.Author.IsDeleted)
        {
            AddInclude(b => b.Author);
            // ApplyPaging(currentPageIndex, take);
            //         (b => b.Title);
        }
    }
}
