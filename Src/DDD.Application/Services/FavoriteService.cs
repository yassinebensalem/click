using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using DDD.Infra.Data.Repository;
using DDD.Infra.Data.Repository.EventSourcing;

namespace DDD.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IMapper _mapper;
        private readonly IFavoriteBookRepository _IFavoriteBookRepository;
        private readonly IFavoriteCategoryRepository _IFavoriteCategoryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public FavoriteService(IMapper mapper, IFavoriteBookRepository iFavoriteBookRepository, IFavoriteCategoryRepository iFavoriteCategoryRepository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _IFavoriteBookRepository = iFavoriteBookRepository;
            _IFavoriteCategoryRepository = iFavoriteCategoryRepository;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }

        public bool AddFavoriteBook(FavoriteBookVM favoriteBookVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewFavoriteBookCommand>(favoriteBookVM);
                Bus.SendCommand(registerCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool AddFavoriteCategory(FavoriteCategoryVM favoriteCategoryVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewFavoriteCategoryCommand>(favoriteCategoryVM);
                Bus.SendCommand(registerCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DeleteFavoriteBook(Guid id)
        {
            try
            {
                var removeCommand = new RemoveFavoriteBookCommand(id);
                Bus.SendCommand(removeCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DeleteFavoriteCategory(Guid id)
        {
            try
            {
                var removeCommand = new RemoveFavoriteCategoryCommand(id);
                Bus.SendCommand(removeCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public IEnumerable<FavoriteBookVM> GetAllFavoriteBooks()
        {
            return _IFavoriteBookRepository.GetAll().ProjectTo<FavoriteBookVM>(_mapper.ConfigurationProvider);

        }
        public IEnumerable<FavoriteCategoryVM> GetAllFavoriteCategories()
        {
            return _IFavoriteCategoryRepository.GetAll().ProjectTo<FavoriteCategoryVM>(_mapper.ConfigurationProvider);

        }

        public IEnumerable<FavoriteBookVM> GetAllFavoriteBooks(int skip, int take)
        {
            return _IFavoriteBookRepository.GetAll(new FavoriteBookFilterPaginatedSpecification(skip, take))
               .ProjectTo<FavoriteBookVM>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<FavoriteCategoryVM> GetAllFavoriteCategories(int skip, int take)
        {
            return _IFavoriteCategoryRepository.GetAll(new FavoriteCategoryFilterPaginatedSpecification(skip, take))
               .ProjectTo<FavoriteCategoryVM>(_mapper.ConfigurationProvider);
        }

        public FavoriteBookVM GetFavoriteBookById(Guid Id)
        {
            return _mapper.Map<FavoriteBookVM>(_IFavoriteBookRepository.GetById(Id));
        }

        public IEnumerable<FavoriteBookVM> GetFavoriteBooksByUserId(string userId)
        {
            return _IFavoriteBookRepository.GetAll(new GetFavoriteBooksByUserIdSpecification(userId))
                 .ProjectTo<FavoriteBookVM>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<FavoriteBookVM> GetFavoriteBookByUserIdAndBookId(string userId, Guid bookId)
        {
            return _IFavoriteBookRepository.GetAll(new UserIdAndBookIdSpecification(userId, bookId))
                .ProjectTo<FavoriteBookVM>(_mapper.ConfigurationProvider);
        }
        public FavoriteCategoryVM GetFavoriteCategoryById(Guid Id)
        {
            return _mapper.Map<FavoriteCategoryVM>(_IFavoriteCategoryRepository.GetById(Id));
        }
         
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
