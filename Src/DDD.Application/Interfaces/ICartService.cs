using System;
using System.Collections.Generic;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
    public interface ICartService : IDisposable
    {
        bool AddBookToCart(CartVM cartVM);
        bool DeleteBookFromCart(Guid id);
        IEnumerable<CartVM> GetAllCarts();
        CartVM GetCartById(Guid Id);
        IEnumerable<CartVM> GetCartsByUserId(string userId);
        IEnumerable<CartVM> GetCartByUserIdAndBookId(string userId, Guid bookId);
    }
}
