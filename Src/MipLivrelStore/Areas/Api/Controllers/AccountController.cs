using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Extensions;
using DDD.Infra.CrossCutting.Identity.Models;
using DDD.Infra.CrossCutting.Identity.Models.AccountViewModels;
using DDD.Infra.CrossCutting.Identity.Services;
using DDD.Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static DDD.Application.Enum.Constants;

namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ApiController
    {
        private readonly IFileManagerLogic _fileManagerLogic;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUser _user;
        private readonly IJwtFactory _jwtFactory;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly IInvoiceService _InvoiceService;
        private readonly IBookService _IBookService;
        private readonly IAuthorService _authorService;
        private readonly IConfiguration _configuration;
        private const string _DEFAULTPASSWORD = "Pas6Sur!";

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            IUser user, IInvoiceService InvoiceService,
            IJwtFactory jwtFactory,
            ILoggerFactory loggerFactory,
            INotificationHandler<DomainNotification> notifications,
            IEmailSender emailSender, IBookService IBookService, IAuthorService authorService,
        IMediatorHandler mediator, IFileManagerLogic fileManagerLogic, IConfiguration configuration) : base(notifications, mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _user = user;
            _jwtFactory = jwtFactory;
            _emailSender = emailSender;
            _fileManagerLogic = fileManagerLogic;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _InvoiceService = InvoiceService;
            _IBookService = IBookService;
            _authorService = authorService;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response();
            }
            // Get User
            var appUser = await _userManager.FindByEmailAsync(model.Email);

            var isAdmin = await _userManager.IsInRoleAsync(appUser, UserRoleVM.Admin.ToString());
            if (!isAdmin)
            {
                NotifyError("Login for Admin only", "Login for Admin only");
                return Response();
            }

            // Sign In
            var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
            if (!signInResult.Succeeded)
            {
                NotifyError(signInResult.ToString(), "Login failure");
                return Response();
            }

            _logger.LogInformation(1, "User logged in.");
            return Response(await GenerateToken(appUser));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response();
            }

            // Add User
            var appUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Birthdate = model.Birthdate,
                PhotoPath = model.PhotoPath,
                Address = model.Address,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                RateOnOriginalPrice = model.RateOnOriginalPrice,
                RateOnSale = model.RateOnSale,
                CountryId = model.CountryId > 0 ? model.CountryId : null,
                EmailConfirmed = model.UserRole == UserRoleVM.Admin,
                IdFiscal = model.IdFiscal,
                RaisonSocial = model.RaisonSocial,
                CreatedAt= DateTime.UtcNow,
            };

            if (model.PhotoFile != null)
            {
                // UplodePhoto
                string fileName = Guid.NewGuid().ToString();
                appUser.PhotoPath = $"{fileName}{Path.GetExtension(model.PhotoFile.FileName)}";
                await _fileManagerLogic.Upload(model.PhotoFile, "bookscover", appUser.PhotoPath);
            }

            if (model.UserRole != UserRoleVM.Admin)
            {
                model.Password = _configuration.GetSection("DefaultPass") != null ? _configuration.GetSection("DefaultPass").Value : _DEFAULTPASSWORD;
                //GeneratePassword();
            }

            var identityResult = await _userManager.CreateAsync(appUser, model.Password);
            if (!identityResult.Succeeded)
            {
                AddIdentityErrors(identityResult);
                return Response();
            }

            // Add UserRoles
            identityResult = await _userManager.AddToRoleAsync(appUser, model.UserRole.ToString());
            if (!identityResult.Succeeded)
            {
                AddIdentityErrors(identityResult);
                return Response();
            }

            //if (model.UserRole != UserRoleVM.Admin)
            //{
            //    //send an email notification to the added user (editor, publisher)
            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            //    var callbackUrl = Url.MyEmailConfirmationLink(appUser.Id, code, Request.Scheme);
            //    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl, model.Password);
            //}

            _logger.LogInformation(3, "User created a new account with password.");
            return Response();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAccount([FromForm] RegisterViewModel model)
        {

            var currentUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            if (currentUser != null)
            {
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.UserName = model.Email;
                currentUser.Address = model.Address;
                currentUser.CountryId = model.CountryId>0?model.CountryId:null;
                currentUser.Gender = model.Gender;
                currentUser.Birthdate = model.Birthdate;
                currentUser.RateOnOriginalPrice = model.RateOnOriginalPrice;
                currentUser.RateOnSale = model.RateOnSale;
                currentUser.IdFiscal = model.IdFiscal;
                currentUser.RaisonSocial = model.RaisonSocial;
                currentUser.isActive = model.IsActive;
                currentUser.PhotoPath = model.PhotoPath != null ? model.PhotoPath : currentUser.PhotoPath;

                if (model.PhotoFile != null)
                {
                    // UplodePhoto
                    string fileName = Guid.NewGuid().ToString();
                    currentUser.PhotoPath = $"{fileName}{Path.GetExtension(model.PhotoFile.FileName)}";

                    await _fileManagerLogic.Upload(model.PhotoFile, "bookscover", currentUser.PhotoPath);
                }

                // UpdateUser
                var updateUser = await _userManager.UpdateAsync(currentUser);

                if (!string.IsNullOrWhiteSpace(model.NewPassword) && !string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                    if (!signInResult.Succeeded)
                    {
                        NotifyError(signInResult.ToString(), "Incorrect current password");
                        return Response();
                    }
                    if (model.NewPassword != model.ConfirmPassword)
                    {
                        NotifyModelStateErrors();
                        var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();
                        return Response();
                    }

                    //UpdatePassword
                    var updatePasswordResult = await _userManager.ChangePasswordAsync(currentUser, model.Password, model.NewPassword);
                    if (updatePasswordResult.Succeeded)
                    {
                        return Response();
                    }
                    else
                    {
                        return Response();
                    }
                }
                else
                {
                    if (updateUser.Succeeded)
                    {
                        return Response();
                    }
                    else
                    {
                        return Response();
                    }
                }
            }
            return Response();
        }

        [HttpPost]
        [Route("sendEmail")]
        public async Task<IActionResult> SendConfirmationEmail([FromForm] string email)
        {
            var currentUser = _userManager.Users.SingleOrDefault(r => r.Email == email);
            if (currentUser != null)
            {
                string password = _configuration.GetSection("DefaultPass") != null ? _configuration.GetSection("DefaultPass").Value : _DEFAULTPASSWORD;
                //    //send an email notification to the added user (editor, publisher)
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(currentUser);
                var callbackUrl = Url.MyEmailConfirmationLink(currentUser.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(email, callbackUrl, password);
            }
            return Response();
        }


        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user is null)
            {
                NotifyError("UserNotFound", "User does not exist");
                return Response();
            }
            else
            {
                var DeleteResult = _userManager.DeleteAsync(user).Result;
                return Response();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response();
            }

            // Get current RefreshToken
            var refreshTokenCurrent = _dbContext.RefreshTokens.SingleOrDefault
                (x => x.Token == model.RefreshToken && !x.Used && !x.Invalidated);
            if (refreshTokenCurrent is null)
            {
                NotifyError("RefreshToken", "Refresh token does not exist");
                return Response();
            }
            if (refreshTokenCurrent.ExpiryDate < DateTime.UtcNow)
            {
                // Update current RefreshToken
                refreshTokenCurrent.Invalidated = true;
                await _dbContext.SaveChangesAsync();
                NotifyError("RefreshToken", "Refresh token invalid");
                return Response();
            }

            // Get User
            var appUser = await _userManager.FindByIdAsync(refreshTokenCurrent.UserId);
            if (appUser is null)
            {
                NotifyError("User", "User does not exist");
                return Response();
            }

            // Remove current RefreshToken
            //_dbContext.Remove(refreshTokenCurrent);
            //await _dbContext.SaveChangesAsync();

            // Update current RefreshToken
            refreshTokenCurrent.Used = true;
            await _dbContext.SaveChangesAsync();

            return Response(await GenerateToken(appUser));
        }

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrent()
        {
            if (_user.IsAuthenticated())
            {
                var name = _user.GetClaimsIdentity().FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name));
                var appUser = await _userManager.FindByEmailAsync(name.Value);
                return Response(new
                {
                    IsAuthenticated = _user.IsAuthenticated(),
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    ClaimsIdentity = _user.GetClaimsIdentity().Select(x => new { x.Type, x.Value }),
                });
            }
            else
            {
                return Response(new
                {
                    IsAuthenticated = _user.IsAuthenticated(),
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    ClaimsIdentity = _user.GetClaimsIdentity().Select(x => new { x.Type, x.Value }),
                });
            }

        }

        private async Task<TokenViewModel> GenerateToken(ApplicationUser appUser)
        {
            // Init ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, appUser.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, appUser.Id));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, appUser.UserName));

            // Get UserClaims
            var userClaims = await _userManager.GetClaimsAsync(appUser);
            claimsIdentity.AddClaims(userClaims);

            // Get UserRoles
            var userRoles = await _userManager.GetRolesAsync(appUser);
            claimsIdentity.AddClaims(userRoles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));
            // ClaimsIdentity.DefaultRoleClaimType & ClaimTypes.Role is the same

            // Get RoleClaims
            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claimsIdentity.AddClaims(roleClaims);
            }

            // Generate access token
            var jwtToken = await _jwtFactory.GenerateJwtToken(claimsIdentity);

            // Add refresh token
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString("N"),
                UserId = appUser.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMinutes(90),
                JwtId = jwtToken.JwtId
            };
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new TokenViewModel
            {
                AccessToken = jwtToken.AccessToken,
                RefreshToken = refreshToken.Token
            };
        }

        [NonAction]
        public string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }

        [HttpGet]
        [Route("ActiveUsersByRole")]
        public IActionResult GetActiveUsersByRole(string role)
        {
            return Response(new { UsersList = _userManager.GetUsersInRoleAsync(role).Result.Where(u => u.isActive) });
        }

        /// <summary>
        /// renvoie le nombre d'inscriptions
        /// non confirmées
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("NonConfirmedSubscriptionsCount")]
        public IActionResult GetPendingSubscriptions()
        {
            return Ok(_userManager.GetUsersInRoleAsync("Subscriber").Result.Where(u => !u.EmailConfirmed).Count());
        }

        [HttpGet]
        [Route("UsersByRole")]
        public IActionResult GetUsersByRole(string role)
        {

            var list = _userManager.GetUsersInRoleAsync(role).Result;

            if (role.ToUpper() == UserRoleVM.Author.ToString().ToUpper())
            {
                var listAuthors = _authorService.GetAll();
                var editorList = _userManager.GetUsersInRoleAsync(UserRoleVM.Editor.ToString()).Result;
                foreach (var _author in list)
                {
                    var singleAuthor = listAuthors.Where(a => a.UserId == Guid.Parse(_author.Id)).FirstOrDefault();
                    if (singleAuthor == null)
                    {
                        _author.BooksNumber = 0;
                        continue;
                    }
                    //get books count
                    var books = _IBookService.GetBooksByAuthorId_WithoutPagination(singleAuthor.Id).ToList();
                    _author.BooksNumber = books.Count;
                    if (books != null && books.Count == 0) continue;

                    var recent_book = books.OrderByDescending(x => x.PublicationDate).FirstOrDefault();
                    //get the related publisher to this author
                    var related_editor = editorList.FirstOrDefault(editor => editor.Id == recent_book.PublisherId);
                    _author.RaisonSocial = related_editor.RaisonSocial;
                }
            }

            return Response(new { UsersList = list });
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult Post([FromBody] InvoiceVM _invoiceVM)
        {
            var AddResult = _InvoiceService.AddInvoice(_invoiceVM);


            return Response(_invoiceVM);
        }

        [HttpGet]
        [Route("ActiveCustomers")]
        public IActionResult GetActiveCostomers()
        {
            var ConnString = _configuration.GetConnectionString("DefaultConnection");
            string SqlString = "";
            var nn = 0;
            SqlString = "Select count(distinct(UserId)) as Numb From Invoices where isDeleted='false'";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(SqlString, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        nn = (int)reader["Numb"];

                    }
                }
            }

            return Response(nn);
        }
    }
}
