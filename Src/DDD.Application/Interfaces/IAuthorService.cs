using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
    public interface IAuthorService : IDisposable
    {
        IEnumerable<Author> GetAll();
        IEnumerable<AuthorVM> GetAllAsVM();
        Task<IEnumerable<AuthorVM>> GetUsedAuthors();
        AuthorVM GetAuthorById(Guid Id);
        string AddAuthors(AuthorVM authorVM);
        bool Update(AuthorVM authorVM);
        AuthorVM GetAuthorByEmail(string email);
        AuthorVM GetAuthorByFullName(string fullName);
        bool DeleteAuthor(Guid id);

    }
}
