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
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _ICartRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public CartService(IMapper mapper, ICartRepository iCartRepository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _ICartRepository = iCartRepository; 
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }

        public bool AddBookToCart(CartVM cartVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewCartCommand>(cartVM);
                Bus.SendCommand(registerCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }
         
        public bool DeleteBookFromCart(Guid id)
        {
            try
            {
                var removeCommand = new RemoveCartCommand(id);
                Bus.SendCommand(removeCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }
          
        public IEnumerable<CartVM> GetAllCarts()
        {
            return _ICartRepository.GetAll().ProjectTo<CartVM>(_mapper.ConfigurationProvider);

        }
           
        public CartVM GetCartById(Guid Id)
        {
            return _mapper.Map<CartVM>(_ICartRepository.GetById(Id));
        }

        public IEnumerable<CartVM> GetCartsByUserId(string userId)
        {
            return _ICartRepository.GetAll(new GetCartsByUserIdSpecification(userId))
                 .ProjectTo<CartVM>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<CartVM> GetCartByUserIdAndBookId(string userId, Guid bookId)
        {
            return _ICartRepository.GetAll(new GetCartByUserIdAndBookIdSpecification(userId, bookId))
                .ProjectTo<CartVM>(_mapper.ConfigurationProvider);
        }
          
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
