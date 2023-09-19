using System;
using System.Collections.Generic;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
    public interface IPromoUserService : IDisposable
    {
        PromoUserVM GetPromoUserById(Guid Id);
        IEnumerable<PromoUserVM> GetAll();
        IEnumerable<PromoUserVM> GetPromoUserByUserId(string userId);
        IEnumerable<PromoUserVM> GetPromoUserByUserIdAndPromoId(string userId, Guid promoId);
        bool AddPromoUser(PromoUserVM promoUserVM);
        bool UpdatePromoUser(PromoUserVM promoUserVM);
        bool DeletePromoUser(Guid id);
    }
}
