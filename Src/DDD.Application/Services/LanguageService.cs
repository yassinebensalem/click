using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
  public class LanguageService : ILanguageService
    {
        private readonly IMapper _mapper;
        private readonly ILanguageRepository _languageRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public LanguageService(IMapper mapper, ILanguageRepository languageRepository, IEventStoreRepository eventStoreRepository,
            IMediatorHandler bus)
        {
            _mapper = mapper;
            _languageRepository = languageRepository;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }

        //start implementing needed services
        public IEnumerable<LanguageViewModel> GetAll()
        {
            return _languageRepository.GetAll().ProjectTo<LanguageViewModel>(_mapper.ConfigurationProvider);
        }

        public LanguageViewModel GetLanguageById(int Id)
        {
            return _mapper.Map<LanguageViewModel>(_languageRepository.GetById(Id));
        }
        public async Task<IEnumerable<LanguageViewModel>> GetUsedLanguages()
        {
            return  _mapper.Map<IEnumerable<LanguageViewModel>>(await _languageRepository.GetUsedLanguages());

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
