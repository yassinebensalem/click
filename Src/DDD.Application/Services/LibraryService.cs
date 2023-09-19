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
    public class LibraryService : ILibraryService
    {
        private readonly IMapper _mapper;
        private readonly ILibraryRepository _ILibraryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public LibraryService(IMapper mapper, ILibraryRepository iLibraryRepository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _ILibraryRepository = iLibraryRepository;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }

        public IEnumerable<LibraryVM> GetAllLibraries()
        {
            return _ILibraryRepository.GetAll().ProjectTo<LibraryVM>(_mapper.ConfigurationProvider);

        }

        public LibraryVM GetLibraryById(Guid Id)
        {
            return _mapper.Map<LibraryVM>(_ILibraryRepository.GetById(Id));
        }

        public IEnumerable<LibraryVM> GetLibrariesByUserId(string userId)
        {
            return _ILibraryRepository.GetAll(new GetLibrariesByUserIdSpecification(userId))
                 .ProjectTo<LibraryVM>(_mapper.ConfigurationProvider);
        }

        public bool AddBookToLibrary(LibraryVM libraryViewModel)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewLibraryCommand>(libraryViewModel);
                Bus.SendCommand(registerCommand); 
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool UpdateCurrentPage(UpdateLibraryVM updateLibraryVM)
        {
            try
            {
                var updateCommand = _mapper.Map<UpdateLibraryCommand>(updateLibraryVM);
                Bus.SendCommand(updateCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DeleteBookFromLibrary(Guid id)
        {
            try
            {
                var removeCommand = new RemoveLibraryCommand(id);
                Bus.SendCommand(removeCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public IEnumerable<LibraryVM> GetLibraryByUserIdAndBookId(string userId, Guid bookId)
        {
            return _ILibraryRepository.GetAll(new GetLibraryByUserIdAndBookIdSpecification(userId, bookId))
                .ProjectTo<LibraryVM>(_mapper.ConfigurationProvider);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
