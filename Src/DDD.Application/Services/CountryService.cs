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
   public class CountryService : ICountryService
    {
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public CountryService(IMapper mapper, ICountryRepository countryRepository, IEventStoreRepository eventStoreRepository,
            IMediatorHandler bus)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }
        public IEnumerable<CountryViewModel> GetAll()
        {
            return _countryRepository.GetAll().ProjectTo<CountryViewModel>(_mapper.ConfigurationProvider);
        }

       

        public CountryViewModel GetCountryById(int Id)
        {
            return _mapper.Map<CountryViewModel>(_countryRepository.GetById(Id));
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
