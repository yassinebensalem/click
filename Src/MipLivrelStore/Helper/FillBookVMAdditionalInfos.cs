using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
//using Azure.Core;
using DDD.Application.ViewModels;
using DDD.Domain.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MipLivrelStore.Helper
{
    public static class FillBookVMAdditionalInfos
    {
        public static List<BookViewModel> FillBookVMList(List<BookViewModel> BooksList, List<AuthorVM> AuthorsList = null,
            List<ApplicationUser> EditorsList = null, List<CategoryViewModel> CategoriesList = null, List<CountryViewModel> CountriesList = null,
             List<LanguageViewModel> LanguagesList = null, List<PrizeBookVM> prizeBookList = null,
            List<FavoriteBookVM> FavoriteList = null, List<CartVM> CartList = null, List<LibraryVM> LibraryList = null,
              List<BookViewModel> DiscountedBooksList = null, List<BookViewModel> FreeBooksList = null)
        {
            var newBookList = new List<BookViewModel>();
            //if (EditorsList == null) return BooksList;
            //var editorIds = EditorsList.Select(e => e.Id);
            //var quickList = BooksList.Where(b => editorIds.Contains(b.PublisherId)).OrderBy(b => b.Title);
            //if (quickList == null) return BooksList;
            //if (quickList.Count() == 0) return BooksList;
            foreach (var book in BooksList/*quickList*/)
            {
                FillBookVMModel(book, AuthorsList, EditorsList, CategoriesList, CountriesList, LanguagesList, prizeBookList, FavoriteList, CartList,
                    LibraryList, DiscountedBooksList, FreeBooksList);
                newBookList.Add(book);
            }
            BooksList = newBookList;
            return newBookList;
        }

        public static void FillBookVMModel(BookViewModel book, List<AuthorVM> AuthorsList = null, List<ApplicationUser> EditorsList = null,
             List<CategoryViewModel> CategoriesList = null, List<CountryViewModel> CountriesList = null, List<LanguageViewModel> LanguagesList = null,
             List<PrizeBookVM> prizeBookList = null, List<FavoriteBookVM> FavoriteList = null,
             List<CartVM> CartList = null, List<LibraryVM> LibraryList = null,
              List<BookViewModel> DiscountedBooksList = null, List<BookViewModel> FreeBooksList = null,
              List<PromotionVM> AllFreePromotions = null, ApplicationUser currentUser = null)
        {
            if (AuthorsList != null)
            {
                if (AuthorsList.Count > 0)
                {
                    var author = AuthorsList.Find(e => e.Id == book.AuthorId);
                    if (author != null)
                    {
                        author.FirstName = !string.IsNullOrEmpty(author.FirstName) ? author.FirstName.Trim() : "";
                        author.LastName = !string.IsNullOrEmpty(author.LastName) ? author.LastName.Trim() : "";
                    }
                    book.AuthorName = (author != null) ? author.AuthorName : "";// $"{author.FirstName} {author.LastName}".Trim();
                }
            }
            if (EditorsList != null)
            {
                if (EditorsList.Count > 0)
                {
                    var editor = EditorsList.Find(e => e.Id == book.PublisherId);
                    if (editor != null)
                    {
                        book.PublisherName = editor.RaisonSocial.Trim();
                        book.BusinessPrice = Math.Round(book.Price * (editor.RateOnOriginalPrice/100), 2);
                    }
                }
            }
            if (CategoriesList != null)
            {
                if (CategoriesList.Count > 0)
                {
                    var category = CategoriesList.Find(e => e.Id == book.CategoryId);
                    if (category != null)
                    {
                        book.CategoryName = category.CategoryName.Trim();
                        category.CategoryName.Trim();
                    }
                }
            }
            if (CountriesList != null)
            {
                if (CountriesList.Count > 0)
                {
                    var country = CountriesList.Find(e => e.Id == book.CountryId);
                    if (country != null) book.CountryName = country.Name;
                }
            }
            if (LanguagesList != null)
            {
                if (LanguagesList.Count > 0)
                {
                    var language = LanguagesList.Find(e => e.Id == book.LanguageId);
                    if (language != null) book.LanguageName = language.Name;
                }
            }
            if (prizeBookList != null)
            {
                if (prizeBookList.Count > 0)
                {
                    var prizeBook = prizeBookList.Find(e => e.BookId == book.Id);
                    if (prizeBook != null) book.AwardEdition = prizeBook.Edition;
                }
            }

            if (FavoriteList != null)
            {
                if (FavoriteList.Count > 0)
                {
                    book.isFavorite = FavoriteList.Find(x => x.BookId == book.Id) != null;
                }
            }

            if (CartList != null)
            {
                if (CartList.Count > 0)
                {
                    book.inCart = CartList.Find(x => x.BookId == book.Id) != null;
                }
            }

            if (LibraryList != null)
            {
                if (LibraryList.Count > 0)
                {
                    book.inLibrary = LibraryList.Find(x => x.BookId == book.Id) != null;
                }
            }
            if (DiscountedBooksList != null)
            {
                if (DiscountedBooksList.Count > 0)
                {
                    var discoutedBook = DiscountedBooksList.Find(x => x.Id == book.Id);
                    book.PromotionsPercentage = discoutedBook != null ? discoutedBook.PromotionsPercentage : book.PromotionsPercentage;
                }
            }
            if (FreeBooksList != null)
            {
                if (FreeBooksList.Count > 0)
                {
                    var FreeBook = FreeBooksList.Find(x => x.Id == book.Id);
                    book.PromotionsPercentage = FreeBook != null ? 100 : book.PromotionsPercentage;
                    //set attribute CanBeAddedToLib of this free book (bookVM)
                    //if the user haven't reached the max of the promotion (PromoUserVM)
                    //get the MaxFree of this promotion (promotionVM)
                    //foreach (var promo in AllFreePromotions)
                    //{
                    //    var promotionBook = promo.PromotionBook.SingleOrDefault(x => x.BookId == book.Id);
                    //    if (promotionBook == null) continue;
                    //    //get PromoUser by promoId and userId

                    //}
                }
            }
            if (!(book.BusinessPrice % 1 == 0))
                book.BusinessPrice = Math.Floor(book.BusinessPrice) + 1;
        }

        public static List<CategoryViewModel> setCategoryName(IEnumerable<CategoryViewModel> _CategoriesList)
        {
            List<CategoryViewModel> newCategoryList = new List<CategoryViewModel>();
            foreach (var category in _CategoriesList)
            {
                Dictionary<string, string> CategoryNames = JsonSerializer.Deserialize<Dictionary<string, string>>(category.CategoryName);
                var cat = CategoryNames.Where(x => x.Key == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName).ToList()[0].Value;
                //category.CategoryName = cat;
                newCategoryList.Add(new CategoryViewModel()
                {
                    Id = category.Id,
                    CategoryName = cat,
                    Status = category.Status
                });
            }
            return newCategoryList;
        }

        public static void SetPromotionOfBooks(List<BookViewModel> BookList, List<BookViewModel> DiscountList = null, List<BookViewModel> FreeList = null)
        {
            foreach (var book in BookList)
            {
                if (DiscountList != null)
                {
                    if (DiscountList.Count > 0)
                    {
                        var discoutedBook = DiscountList.Find(x => x.Id == book.Id);
                        book.PromotionsPercentage = discoutedBook != null ? discoutedBook.PromotionsPercentage : book.PromotionsPercentage;
                    }
                }
                if (FreeList != null)
                {
                    if (FreeList.Count > 0)
                    {
                        var FreeBook = FreeList.Find(x => x.Id == book.Id);
                        book.PromotionsPercentage = FreeBook != null ? 100 : book.PromotionsPercentage;
                    }
                }
            }
        }

        public static void SetDiscountForBooks(List<BookViewModel> BookList, List<BookViewModel> DiscountList)
        {
            foreach (var book in BookList)
            {
                var discoutedBook = DiscountList.Find(x => x.Id == book.Id);
                book.PromotionsPercentage = discoutedBook != null ? discoutedBook.PromotionsPercentage : book.PromotionsPercentage;
            }
        }

        public static void SetFreeForBooks(List<BookViewModel> BookList, List<BookViewModel> FreeList)
        {
            foreach (var book in BookList)
            {
                var FreeBook = FreeList.Find(x => x.Id == book.Id);
                book.PromotionsPercentage = FreeBook != null ? 100 : book.PromotionsPercentage;
            }
        }

        public static void SetFavoriteAndCartFlags(List<BookViewModel> BookList, List<FavoriteBookVM> FavoriteList, List<CartVM> CartList)
        {
            foreach (var book in BookList)
            {
                book.isFavorite = FavoriteList.Find(x => x.BookId == book.Id) != null;
                book.inCart = CartList.Find(x => x.BookId == book.Id) != null;
            }
        }


    }
}
