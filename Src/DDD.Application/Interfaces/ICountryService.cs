using System;
using System.Collections.Generic;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.ViewModels;
using DDD.Domain.Models;

namespace DDD.Application.Interfaces
{
  public  interface ICountryService : IDisposable
    {
        IEnumerable<CountryViewModel> GetAll();
        CountryViewModel GetCountryById(int Id);

    }
}
