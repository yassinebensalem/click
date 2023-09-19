using System;
using System.Collections.Generic;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;

namespace DDD.Application.Interfaces
{
    public interface ILibraryService : IDisposable
    {
        IEnumerable<LibraryVM> GetAllLibraries();
        LibraryVM GetLibraryById(Guid Id);
        IEnumerable<LibraryVM> GetLibrariesByUserId(string userId);
        bool AddBookToLibrary(LibraryVM libraryViewModel);
        bool UpdateCurrentPage(UpdateLibraryVM updateLibraryVM);
        bool DeleteBookFromLibrary(Guid id);
        IEnumerable<LibraryVM> GetLibraryByUserIdAndBookId(string userId, Guid bookId);
    }
}
