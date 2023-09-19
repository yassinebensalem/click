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
   public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }

        //start implementing needed services
        public IEnumerable<CategoryViewModel> GetAll()
        {
            return _categoryRepository.GetAll().ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<CategoryViewModel> GetAll(int skip, int take)
        {
            return _categoryRepository.GetAll(new CategoryFilterPaginatedSpecification(skip, take))
                .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<CategoryViewModel> GetAllUsedCategories()
        {
            return _categoryRepository.GetAll(new UsedCategorySpecification())
                .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider);
        }
        
        public CategoryViewModel GetCategoryById(Guid Id)
        {
            return _mapper.Map<CategoryViewModel>(_categoryRepository.GetById(Id));
        }

        public bool AddCategory(CategoryViewModel categoryViewModel)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewCategoryCommand>(categoryViewModel);
                Bus.SendCommand(registerCommand);
                //_bookRepository.Add(bookViewModel);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool UpdateCategory(CategoryViewModel categoryViewModel)
        {
            try
            {
                var updateCommand = _mapper.Map<UpdateCategoryCommand>(categoryViewModel);
                Bus.SendCommand(updateCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DeleteCategory(Guid Id)
        {
            try
            {
                var removeCommand = new RemoveCategoryCommand(Id);
                Bus.SendCommand(removeCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public IList<CategoryHistoryData> GetAllHistory(Guid id)
        {
            return CategoryHistory.ToJavaScriptCategoryHistory(_eventStoreRepository.All(id));
        }


        public List<CategoryViewModel> ListCategory(string CategoryName )
        {
            return null;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
