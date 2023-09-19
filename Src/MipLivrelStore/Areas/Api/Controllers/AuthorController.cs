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
using System.IO;
using System.Threading.Tasks;
using DDD.Application.ILogics;

namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorController : ApiController
    {
        private readonly IAuthorService _authorService;
        private readonly IFileManagerLogic _fileManagerLogic;

        public AuthorController(
            IAuthorService authorService,
            IFileManagerLogic fileManagerLogic,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _authorService = authorService;
            _fileManagerLogic = fileManagerLogic;
        }

        [HttpGet]
        [Authorize( Roles = Roles.Admin)]
        [Route("getAll")]
        public IActionResult Get()
        {
            return Response(_authorService.GetAll());
            //return Response(_authorService.GetAllAsVM());
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Route("getById")]
        public IActionResult Get(Guid id)
        {
            var authorViewModel = _authorService.GetAuthorById(id);

            return Response(authorViewModel);
        }
         
        //[HttpPut]
        // [Authorize( Roles = Roles.Admin)]
        //[Route("Update")]
        //public IActionResult Put([FromBody] CategoryViewModel categoryViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        NotifyModelStateErrors();
        //        return Response(categoryViewModel);
        //    }

        //    _authorService.(categoryViewModel);

        //    return Response(categoryViewModel);
        //}

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]

        [Route("Delete")]
        public IActionResult Delete(Guid id)
        {
            var DeleteResult = _authorService.DeleteAuthor(id);

            return Response();
        }
         
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("Add")]

        public async Task<IActionResult> Post([FromForm] AuthorVM authorViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(authorViewModel);
            }
            
            var existingAuthor = _authorService.GetAuthorByFullName($"{authorViewModel.FirstName} {authorViewModel.LastName}");
            if (existingAuthor != null)
            { 
                NotifyError("Author_Already_Exists", "This Author is Already Exists");
                return Response();
            }

            string fileName = Guid.NewGuid().ToString();
            if (authorViewModel.PhotoFile != null)
            {
                // UplodePhoto
                authorViewModel.PhotoPath = $"{fileName}{Path.GetExtension(authorViewModel.PhotoFile.FileName)}";
               await  _fileManagerLogic.Upload(authorViewModel.PhotoFile, "bookscover", authorViewModel.PhotoPath);
            }

            _authorService.AddAuthors(authorViewModel);

            return Response(authorViewModel);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAccount([FromForm] AuthorVM model)
        {
            var currentUser = _authorService.GetAuthorById(model.Id);
            if (currentUser != null)
            {
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.Email = model.Email;
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.Biography = model.Biography;
                currentUser.Birthdate = model.Birthdate;
                if(model.PhotoPath != null)
                {
                    currentUser.PhotoPath = model.PhotoPath;
                }
                
                currentUser.CountryId = model.CountryId;

                if (model.PhotoFile != null)
                {
                    // UplodePhoto
                    string fileName = Guid.NewGuid().ToString();
                    currentUser.PhotoPath = $"{fileName}{Path.GetExtension(model.PhotoFile.FileName)}";
                    await _fileManagerLogic.Upload(model.PhotoFile, "bookscover", currentUser.PhotoPath);
                }

                // UpdateUser
                var updateUser = _authorService.Update(currentUser);
            }
            return Response(currentUser);
        } 
    }
}
