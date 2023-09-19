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

namespace DDD.Application.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler Bus;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IPromotionBookRepository _promotionBookRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookService _bookService;

        public PromotionService(IMapper mapper, IMediatorHandler bus, IEventStoreRepository eventStoreRepository, IPromotionRepository promotionRepository,
            IPromotionBookRepository promotionBookRepository, IBookRepository bookRepository, IBookService bookService)
        {
            _mapper = mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _promotionRepository = promotionRepository;
            _promotionBookRepository = promotionBookRepository;
            _promotionRepository = promotionRepository;
            _bookService = bookService;
        }

        public IEnumerable<PromotionVM> GetAll()
        {
            return _promotionRepository.GetAll().ProjectTo<PromotionVM>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<PromotionVM> GetAll(int skip, int take)
        {
            return _promotionRepository.GetAll(new PromotionsPaginatedSpecification(skip, take))
                .ProjectTo<PromotionVM>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<PromotionVM> GetAllFreePromotions()
        {
            return _promotionRepository.GetAll(new FreePromotionsSpecification())
                .ProjectTo<PromotionVM>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<PromotionBookVM> GetAllPromotionBook()
        {
            return _promotionBookRepository.GetAll().ProjectTo<PromotionBookVM>(_mapper.ConfigurationProvider);
        }

        public PromotionVM GetPromotionById(Guid Id)
        {
            return _mapper.Map<PromotionVM>(_promotionRepository.GetById(Id));
        }

        public IEnumerable<PromotionVM> GetPromotionsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _promotionRepository.GetAll(new PromotionsByDateRangeSpecification(startDate, endDate))
                .ProjectTo<PromotionVM>(_mapper.ConfigurationProvider);
        }

        public List<BookViewModel> GetBooksInPromotion(Guid PromotionId)
        {
            var listBooksIDs = _promotionBookRepository.GetAll(new BooksInPromotionSpecification(PromotionId)).Select(x => x.BookId);
            var listBooks = new List<BookViewModel>();
            foreach (var id in listBooksIDs)
            {
                var book = _bookService.GetBookById(id);
                listBooks.Add(book);
            }
            return listBooks;
        }

        public List<BookViewModel> GetFreeBooks(int skip, int take)
        {
            var listBooks = _mapper.Map<List<BookViewModel>>(_promotionRepository.GetFreeBooks());
            listBooks.ForEach(b => b.PromotionsPercentage = 100);
            return listBooks;
        }
        public List<BookViewModel> _GetFreeBooks(int skip, int take)
        {
            var listBooks = _mapper.Map<List<BookViewModel>>(_promotionRepository.GetFreeBooks(skip,take));
            listBooks.ForEach(b => b.PromotionsPercentage = 100);
            return listBooks;
        }
        public List<BookViewModel> GetDiscountBooks(int skip, int take)
        {
            var listPromotions = _promotionRepository.GetAll(new DiscountBooksPromotionSpecification(skip, take));
            var listPromotionBooks = listPromotions.Select(x => x.PromotionBook);
            var listBookIDs = new List<Guid>();
            var listBooks = new List<BookViewModel>();

            foreach (var pbCollection in listPromotionBooks)
            {
                foreach (var pb in pbCollection)
                {
                    //listBookIDs.Add(x.BookId);
                    var book = _bookService.GetBookById(pb.BookId);
                    if (book.IsDeleted) continue;
                    var promo = listPromotions.Where(p => p.Id == pb.PromotionId).FirstOrDefault();
                    book.PromotionsPercentage = (int)promo.Percentage;
                    listBooks.Add(book);
                }
            }

            return listBooks;
        }

        public bool AddPromotion(PromotionVM promotionVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewPromotionCommand>(promotionVM);
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

        public bool AddPromotionBook(PromotionBookVM promotionBookVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewPromotionBookCommand>(promotionBookVM);
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

        public bool UpdatePromotion(PromotionVM promotionVM)
        {
            try
            {
                var updateCommand = _mapper.Map<UpdatePromotionCommand>(promotionVM);
                Bus.SendCommand(updateCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DeletePromotion(Guid Id)
        {
            try
            {
                var removeCommand = new RemovePromotionCommand(Id);
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
