using System;
using System.Collections.Generic;
using System.Linq;
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
using DDD.Infra.Data.Repository;
using DDD.Infra.Data.Repository.EventSourcing;

namespace DDD.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;
        //private readonly IConfiguration _configuration;
        public AuthorService(IMapper mapper, IAuthorRepository authorRepository, IBookRepository bookRepository, /*IConfiguration configuration,*/ IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            //_configuration = configuration;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }
        public IEnumerable<Author> GetAll()
        {
            //GetAllAuthorsSpecification
            return _authorRepository.GetAll(new GetAllAuthorsSpecification());
        }

        public IEnumerable<AuthorVM> GetAllAsVM()
        {
            try {
                return _authorRepository.GetAll().ProjectTo<AuthorVM>(_mapper.ConfigurationProvider);
            }
            catch (Exception ex)
            {
                return new List<AuthorVM>();
            }
        }

        public async Task<IEnumerable<AuthorVM>> GetUsedAuthors()
        {
           return  _mapper.Map<IEnumerable<AuthorVM>>(await _authorRepository.GetUsedAuthors());
        } 
        public AuthorVM GetAuthorById(Guid Id)
        {
            return _mapper.Map<AuthorVM>(_authorRepository.GetById(Id));
        }

        public string AddAuthors(AuthorVM authorVM)
        {
            try
            {
                authorVM.Id = Guid.NewGuid();
                var registerCommand = _mapper.Map<RegisterNewAuthorCommand>(authorVM);
                Bus.SendCommand(registerCommand);
                return authorVM.Id.ToString();
            }
            catch (Exception exp)
            {
                return null;
            }
        }
        public bool Update(AuthorVM authorVM)
        {
            try
            {
                var updateCommand = _mapper.Map<UpdateAuthorCommand>(authorVM);
                Bus.SendCommand(updateCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public AuthorVM GetAuthorByEmail(string email)
        {
            var list = _authorRepository.GetAll(new AuthorByEmailSpecification(email))
                .ProjectTo<AuthorVM>(_mapper.ConfigurationProvider);

            return list.FirstOrDefault();
        }

        public AuthorVM GetAuthorByFullName(string fullName)
        {
            var list = _authorRepository.GetAll(new AuthorByFullNameSpecification(fullName))
                .ProjectTo<AuthorVM>(_mapper.ConfigurationProvider);

            return list.FirstOrDefault();
        }

        public bool DeleteAuthor(Guid Id)
        {
            try
            {
                var removeCommand = new RemoveAuthorCommand(Id);
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
