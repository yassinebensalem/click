using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using DDD.Infra.Data.Repository.EventSourcing;
using static DDD.Domain.Specifications.PrizeSpecification;

namespace DDD.Application.Services
{
    public class PrizeService : IPrizeService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler Bus;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IPrizeRepository prizeRepository;
        private readonly IPrizeBookRepository prizeBookRepository;
        private readonly IBookRepository _bookRepository;

        public PrizeService(IMapper mapper, IMediatorHandler bus, IEventStoreRepository eventStoreRepository, IPrizeRepository _prizeRepository, IPrizeBookRepository _prizeBookRepository, IBookRepository bookRepository)
        {
            _mapper = mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            prizeRepository = _prizeRepository;
            prizeBookRepository = _prizeBookRepository;
            _bookRepository = bookRepository;
        }

        public bool AddPrize(PrizeVM prizeVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewPrizeCommand>(prizeVM);
                Bus.SendCommand(registerCommand);
                //_dbContext.SaveChanges();
                //_bookRepository.Add(bookViewModel);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool AddPrizeBook(PrizeBookVM prizeBookVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewPrizeBookCommand>(prizeBookVM);
                Bus.SendCommand(registerCommand);
                //_dbContext.SaveChanges();
                //_bookRepository.Add(bookViewModel);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public IEnumerable<PrizeVM> GetAll()
        {
            return prizeRepository.GetAll().ProjectTo<PrizeVM>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<PrizeBookVM> GetAllPrizeBook()
        {
            return prizeBookRepository.GetAll().ProjectTo<PrizeBookVM>(_mapper.ConfigurationProvider);
        }

        //public IEnumerable<BookViewModel> GetBookByTitleAndDescription(string Book, int CurrentPageIndex, int take)
        //{
        //    var list =_bookRepository.GetAll(new BookTitleAndDescriptionSpecification(Book, CurrentPageIndex, take))
        //        .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        //    return list;
        //}

        public IEnumerable<BookViewModel> GetBookByTitle(PrizePostVM prizePostVM)
        {
            return _bookRepository.GetAll(new BookTitle(prizePostVM.Title)).ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<PrizeBookVM> GetBookByPrize(BookByPrize bookByPrize)
        {
            return prizeBookRepository.GetAll(new BookbyPrizeSpecifiacation(bookByPrize.edition))
                .ProjectTo<PrizeBookVM>(_mapper.ConfigurationProvider);
        }

        public PrizeVM GetPrizeById(Guid Id)
        {
            return _mapper.Map<PrizeVM>(prizeRepository.GetById(Id));
        }

        public List<PrizeBookVM> ListEdition(string Edition)
        {
            return null;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
