using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Commands;
using DDD.Domain.Common.JSONColumns;
using DDD.Domain.Core.Bus;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using DDD.Infra.Data.Repository.EventSourcing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DDD.Application.Services
{
    public class JoinRequestService : IJoinRequestService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler Bus;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IJoinRequestRepository _joinRequestRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILibraryService _libraryService;


        public JoinRequestService(IMapper mapper, IMediatorHandler bus, IEventStoreRepository eventStoreRepository, IJoinRequestRepository joinRequestRepository, IBookRepository bookRepository, ILibraryService libraryService)
        {
            _mapper = mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _joinRequestRepository = joinRequestRepository;
            _bookRepository = bookRepository;
            _libraryService = libraryService;
        }

        public bool AddJoinRequest(JoinRequestVM joinRequestVM)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewJoinRequestCommand>(joinRequestVM);
                registerCommand.DesiredBooks = _mapper.Map<List<BookCompetition>>(joinRequestVM.DesiredBooks);
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

        public IEnumerable<JoinRequestVM> GetAll(JoinRequestPostVM joinRequestPostVM)
        {
            DateTime fromDate;
            DateTime toDate;
            DateTime.TryParse(joinRequestPostVM.FromDate, out fromDate);
            DateTime.TryParse(joinRequestPostVM.ToDate, out toDate);
            var result = _joinRequestRepository.GetAll(new JoinRequestIntervalSpecification(fromDate, toDate, joinRequestPostVM.RequesterType)).ToList();

            var res1 = _mapper.Map<List<JoinRequestVM>>(result);

            if (result != null)
            {
                res1.ForEach(j =>
                {
                    var books = result.Where(r => r.Id == j.Id).FirstOrDefault().DesiredBooks;
                    if (books != null)
                        j.DesiredBooks = JsonConvert.DeserializeObject<List<BookCompetitionVM>>(result.Where(r => r.Id == j.Id).FirstOrDefault().DesiredBooks);
                });
            }
            return res1;
        }

        public JoinRequestVM GetById(string requestId)
        {

            var resultModel = _joinRequestRepository.GetById(new Guid(requestId));

            var resultVM = _mapper.Map<JoinRequestVM>(resultModel);

            if (resultModel != null)
            {
                if (resultModel.DesiredBooks != null)
                    resultVM.DesiredBooks = JsonConvert.DeserializeObject<List<BookCompetitionVM>>(resultModel.DesiredBooks);
            }
            return resultVM;
        }
        public bool UpdateJoinRequest(JoinRequestVM joinRequestVM)
        {
            try
            {
                var updateCommand = _mapper.Map<UpdateJoinRequestCommand>(joinRequestVM);
                Bus.SendCommand(updateCommand);
                //_bookRepository.Update(b);     
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public JoinRequestVM GetJoinRequestByEmail(string email)
        {
            var list = _joinRequestRepository.GetAll(new JoinRequestByEmailSpecification(email))
                .ProjectTo<JoinRequestVM>(_mapper.ConfigurationProvider);

            return list.FirstOrDefault();
        }

        public bool DeleteCompetitionBook(string requestId, string bookId)
        {
            var joinRequest = _joinRequestRepository.GetById(new Guid(requestId));
            if (joinRequest != null)
            {
                var joinRequestVM = _mapper.Map<JoinRequestVM>(joinRequest);
                if (joinRequestVM.DesiredBooks != null)
                {
                    joinRequestVM.DesiredBooks = JsonConvert.DeserializeObject<List<BookCompetitionVM>>(joinRequest.DesiredBooks);
                    var bookToDelete = joinRequestVM.DesiredBooks.Where(b => b.Id == bookId).FirstOrDefault();
                    if (bookToDelete != null)
                    {
                        if (joinRequestVM.DesiredBooks.Remove(bookToDelete))
                        {
                            var registerCommand = _mapper.Map<UpdateJoinRequestCommand>(joinRequestVM);
                            registerCommand.DesiredBooks = _mapper.Map<List<BookCompetition>>(joinRequestVM.DesiredBooks);
                            Bus.SendCommand(registerCommand);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool AddCompetitionBook(string requestId, string bookId)
        {
            var joinRequest = _joinRequestRepository.GetById(new Guid(requestId));
            if (joinRequest != null)
            {
                var joinRequestVM = _mapper.Map<JoinRequestVM>(joinRequest);
                if (joinRequestVM.DesiredBooks != null)
                {
                    joinRequestVM.DesiredBooks = JsonConvert.DeserializeObject<List<BookCompetitionVM>>(joinRequest.DesiredBooks);
                    var bookToAdd = _bookRepository.GetById(new Guid(bookId));
                    if (bookToAdd != null)
                    {
                        joinRequestVM.DesiredBooks.Add(new BookCompetitionVM { Id = bookId, Title = bookToAdd.Title });
                        var registerCommand = _mapper.Map<UpdateJoinRequestCommand>(joinRequestVM);
                        registerCommand.DesiredBooks = _mapper.Map<List<BookCompetition>>(joinRequestVM.DesiredBooks);
                        Bus.SendCommand(registerCommand);
                        return true;
                    }
                }
            }
            return false;
        }
        public bool GivewayBook(string requestId, string userId)

        {
            var joinRequest = _joinRequestRepository.GetById(new Guid(requestId));
            if (joinRequest != null)
            {
                var joinRequestVM = _mapper.Map<JoinRequestVM>(joinRequest);
                joinRequestVM.DesiredBooks = JsonConvert.DeserializeObject<List<BookCompetitionVM>>(joinRequest.DesiredBooks);

                if (joinRequestVM.DesiredBooks.Any())
                {

                    joinRequestVM.DesiredBooks.ForEach(book =>
                    {
                        if (_libraryService.GetLibraryByUserIdAndBookId(userId, new Guid(book.Id)).FirstOrDefault() == null)
                        {
                            LibraryVM libraryVM = new LibraryVM();
                            libraryVM.Id = Guid.NewGuid();
                            libraryVM.BookId = new Guid(book.Id);
                            libraryVM.UserId = userId;
                            try
                            {
                                var registerCommand = _mapper.Map<RegisterNewLibraryCommand>(libraryVM);
                                Bus.SendCommand(registerCommand);
                            }
                            catch
                            {

                            }
                        }
                    });
                    joinRequestVM.Status = DDD.Domain.Common.Constants.GlobalConstant.JoinRequestState.Accepted;
                    UpdateJoinRequest(joinRequestVM);
                    return true;
                }
            }
            return false;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
