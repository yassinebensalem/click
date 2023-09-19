using System;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Infra.CrossCutting.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(
            ICategoryService categoryService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getAll")]
        public IActionResult Get()
        {
            return Response(_categoryService.GetAll());
        }

        [HttpGet]
        //[AllowAnonymous]
        //[Route("book-managment/{id:guid}")]
        [Route("getById")]
        public IActionResult Get(Guid id)
        {
            var categoryViewModel = _categoryService.GetCategoryById(id);

            return Response(categoryViewModel);
        }

        [HttpGet]
        // [AllowAnonymous]
        [Route("pagination")]
        public IActionResult GetByPageIndex(int skip, int take)
        {
            return Response(_categoryService.GetAll(skip, take));
        }
         
        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [Route("Update")]
        public IActionResult Put([FromBody] CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(categoryViewModel);
            }

            _categoryService.UpdateCategory(categoryViewModel);

            return Response(categoryViewModel);
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        [Route("Delete")]
        public IActionResult Delete(Guid id)
        {
            var DeleteResult = _categoryService.DeleteCategory(id);

            return Response();
        }
         
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("Add")] 
        public IActionResult Post([FromBody] CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(categoryViewModel);
            }

            var AddResult = _categoryService.AddCategory(categoryViewModel);

            return Response(categoryViewModel);
        } 
    }
}
