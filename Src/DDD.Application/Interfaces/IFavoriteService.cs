using System;
using System.Collections.Generic;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
    public interface IFavoriteService : IDisposable
    {
        bool AddFavoriteBook(FavoriteBookVM favoriteBookVM);
        bool AddFavoriteCategory(FavoriteCategoryVM favoriteCategoryVM);
        bool DeleteFavoriteBook(Guid id);
        bool DeleteFavoriteCategory(Guid id);
        IEnumerable<FavoriteBookVM> GetAllFavoriteBooks();
        IEnumerable<FavoriteCategoryVM> GetAllFavoriteCategories();
        IEnumerable<FavoriteBookVM> GetAllFavoriteBooks(int skip, int take);
        IEnumerable<FavoriteCategoryVM> GetAllFavoriteCategories(int skip, int take);
        FavoriteBookVM GetFavoriteBookById(Guid Id);
        IEnumerable<FavoriteBookVM> GetFavoriteBooksByUserId(string userId);
        IEnumerable<FavoriteBookVM> GetFavoriteBookByUserIdAndBookId(string userId,Guid bookId);
        FavoriteCategoryVM GetFavoriteCategoryById(Guid Id);
    }
}
