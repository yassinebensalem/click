using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class PromoUserService : IPromoUserService
    {
        private readonly IMapper _mapper;
        private readonly IPromoUserRepository _IPromoUserRepository; 
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public PromoUserService(IMapper mapper, IPromoUserRepository iPromoUserRepository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _IPromoUserRepository = iPromoUserRepository;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }

        public IEnumerable<PromoUserVM> GetAll()
        {
            return _IPromoUserRepository.GetAll().ProjectTo<PromoUserVM>(_mapper.ConfigurationProvider);

        }

        public PromoUserVM GetPromoUserById(Guid Id)
        {
            return _mapper.Map<PromoUserVM>(_IPromoUserRepository.GetById(Id));

        }

        public IEnumerable<PromoUserVM> GetPromoUserByUserIdAndPromoId(string userId, Guid promoId)
        {
            return _IPromoUserRepository.GetAll(new GetUserIdAndPromoIdSpecification(userId, promoId))
                  .ProjectTo<PromoUserVM>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<PromoUserVM> GetPromoUserByUserId(string userId)
        {
            return _IPromoUserRepository.GetAll(new GetPromoUserByUserIdSpecification(userId))
                 .ProjectTo<PromoUserVM>(_mapper.ConfigurationProvider);
        }
         
        public bool AddPromoUser(PromoUserVM promoUserVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewPromoUserCommand>(promoUserVM);
                Bus.SendCommand(registerCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool UpdatePromoUser(PromoUserVM promoUserVM)
        {
            try
            {
                var updateCommand = _mapper.Map<UpdatePromoUserCommand>(promoUserVM);
                Bus.SendCommand(updateCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DeletePromoUser(Guid id)
        {
            try
            {
                var removeCommand = new RemovePromoUserCommand(id);
                Bus.SendCommand(removeCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
