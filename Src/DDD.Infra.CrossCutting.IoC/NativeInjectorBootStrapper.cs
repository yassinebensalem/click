using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.Logics;
using DDD.Application.Services;
using DDD.Domain.CommandHandlers;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Events;
using DDD.Domain.Core.Notifications;
using DDD.Domain.EventHandlers;
using DDD.Domain.Events;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Services;
using DDD.Infra.CrossCutting.Bus;
using DDD.Infra.CrossCutting.Identity.Authorization;
using DDD.Infra.CrossCutting.Identity.Models;
using DDD.Infra.CrossCutting.Identity.Services;
using DDD.Infra.Data.EventSourcing;
using DDD.Infra.Data.Repository;
using DDD.Infra.Data.Repository.EventSourcing;
using DDD.Infra.Data.UoW;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IJoinRequestService, JoinRequestService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPrizeService, PrizeService>();
            services.AddScoped<ILibraryService, LibraryService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IPromoUserService, PromoUserService>();
            services.AddScoped<ICommunityService, CommunityService>();
            services.AddScoped<IWalletTransactionService, WalletTransactionService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerUpdatedEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerRemovedEvent>, CustomerEventHandler>();

            services.AddScoped<INotificationHandler<BookRegisteredEvent>, BookEventHandler>();
            services.AddScoped<INotificationHandler<BookUpdatedEvent>, BookEventHandler>();
            services.AddScoped<INotificationHandler<BookRemovedEvent>, BookEventHandler>();

            services.AddScoped<INotificationHandler<CategoryRegisteredEvent>, CategoryEventHandler>();
            services.AddScoped<INotificationHandler<CategoryUpdatedEvent>, CategoryEventHandler>();
            services.AddScoped<INotificationHandler<CategoryRemovedEvent>, CategoryEventHandler>();

            services.AddScoped<INotificationHandler<AuthorRegisteredEvent>, AuthorEventHandler>();

            services.AddScoped<INotificationHandler<JoinRequestRegisteredEvent>,JoinRequestEventHandler>();
            services.AddScoped<INotificationHandler<JoinRequestUpdatedEvent>, JoinRequestEventHandler>();
            services.AddScoped<INotificationHandler<JoinRequestRemovedEvent>, JoinRequestEventHandler>();

            services.AddScoped<INotificationHandler<FavoriteCategoryRegisteredEvent>, FavoriteCategoryEventHandler>();
            services.AddScoped<INotificationHandler<FavoriteCategoryRemovedEvent>, FavoriteCategoryEventHandler>();

            services.AddScoped<INotificationHandler<FavoriteBookRegisteredEvent>, FavoriteBookEventHandler>();
            services.AddScoped<INotificationHandler<FavoriteBookRemovedEvent>, FavoriteBookEventHandler>();
             
            services.AddScoped<INotificationHandler<CartRegisteredEvent>, CartEventHandler>();
            services.AddScoped<INotificationHandler<CartRemovedEvent>, CartEventHandler>();

            services.AddScoped<INotificationHandler<PrizeRegisteredEvent>, PrizeEventHandler>();
            services.AddScoped<INotificationHandler<PrizeBookRegisteredEvent>, PrizeEventHandler>();

            services.AddScoped<INotificationHandler<PromotionRegisteredEvent>, PromotionEventHandler>();
            services.AddScoped<INotificationHandler<PromotionBookRegisteredEvent>, PromotionEventHandler>();

            services.AddScoped<INotificationHandler<PromotionRegisteredEvent>, PromotionEventHandler>();
            services.AddScoped<INotificationHandler<PromotionBookRegisteredEvent>, PromotionEventHandler>();

            services.AddScoped<INotificationHandler<CommunityRegisteredEvent>, CommunityEventHandler>();
            services.AddScoped<INotificationHandler<CommunityUpdatedEvent>, CommunityEventHandler>();
            services.AddScoped<INotificationHandler<CommunityRemovedEvent>, CommunityEventHandler>();

            services.AddScoped<INotificationHandler<CommunityMemberEvent>, CommunityMemberEventHandler>();
            services.AddScoped<INotificationHandler<CommunityMemberAssociatedEvent>, CommunityMemberEventHandler>();
            services.AddScoped<INotificationHandler<CommunityMemberDissociatedEvent>, CommunityMemberEventHandler>();

            services.AddScoped<INotificationHandler<WalletTransactionEvent>, WalletTransactionEventHandler>();
            services.AddScoped<INotificationHandler<WalletTransactionRefilledEvent>, WalletTransactionEventHandler>();
            services.AddScoped<INotificationHandler<WalletTransactionWithdrawnEvent>, WalletTransactionEventHandler>();


            services.AddScoped<IRequestHandler<RegisterNewCustomerCommand, bool>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCustomerCommand, bool>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveCustomerCommand, bool>, CustomerCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewBookCommand, bool>, BookCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateBookCommand, bool>, BookCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveBookCommand, bool>, BookCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateBookStateCommand, bool>, BookCommandHandler>();


            services.AddScoped<IRequestHandler<RegisterNewCategoryCommand, bool>, CategoryCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCategoryCommand, bool>, CategoryCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveCategoryCommand, bool>, CategoryCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterNewAuthorCommand, bool>, AuthorCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateAuthorCommand, bool>, AuthorCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveAuthorCommand, bool>, AuthorCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterNewJoinRequestCommand, bool>, JoinRequestCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateJoinRequestCommand, bool>, JoinRequestCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveJoinRequestCommand, bool>, JoinRequestCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewFavoriteBookCommand, bool>, FavoriteBookCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveFavoriteBookCommand, bool>, FavoriteBookCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewFavoriteCategoryCommand, bool>, FavoriteCategoryCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveFavoriteCategoryCommand, bool>, FavoriteCategoryCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewCartCommand, bool>, CartCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveCartCommand, bool>, CartCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewPrizeCommand, bool>, PrizeCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePrizeCommand, bool>, PrizeCommandHandler>();
            services.AddScoped<IRequestHandler<RemovePrizeCommand, bool>, PrizeCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterNewPrizeBookCommand, bool>, PrizeCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewPromotionCommand, bool>, PromotionCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePromotionCommand, bool>, PromotionCommandHandler>();
            services.AddScoped<IRequestHandler<RemovePromotionCommand, bool>, PromotionCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterNewPromotionBookCommand, bool>, PromotionCommandHandler>();

            services.AddScoped<IRequestHandler<AddNewInvoiceCommand, bool>, InvoiceCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterNewLibraryCommand, bool>, LibraryCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveLibraryCommand, bool>, LibraryCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateLibraryCommand, bool>, LibraryCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewPromoUserCommand, bool>, PromoUserCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePromoUserCommand, bool>, PromoUserCommandHandler>();
            services.AddScoped<IRequestHandler<RemovePromoUserCommand, bool>, PromoUserCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewCommunityCommand, bool>, CommunityCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCommunityCommand, bool>, CommunityCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveCommunityCommand, bool>, CommunityCommandHandler>();

            services.AddScoped<IRequestHandler<AssociateCommunityMemberCommand, bool>, CommunityMemberCommandHandler>();
            services.AddScoped<IRequestHandler<DissociateCommunityMemberCommand, bool>, CommunityMemberCommandHandler>();
            services.AddScoped<IRequestHandler<InviteToCommunityWithMembershipCommand, bool>, CommunityMemberCommandHandler>();

            services.AddScoped<IRequestHandler<RefillWalletTransactionCommand, bool>, WalletTransactionCommandHandler>();
            services.AddScoped<IRequestHandler<WithdrawWalletTransactionCommand, bool>, WalletTransactionCommandHandler>();


            // Domain - 3rd parties
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IMailService, MailService>();

            // Infra - Data
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IJoinRequestRepository, JoinRequestRepository>();
            services.AddScoped<IFavoriteBookRepository, FavoriteBookRepository>();
            services.AddScoped<IFavoriteCategoryRepository, FavoriteCategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<IPrizeRepository, PrizeRepository>();
            services.AddScoped<IPrizeBookRepository, BookPrizeRepository>();

            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IPromotionBookRepository, PromotionBookRepository>();
            services.AddScoped<IPromoUserRepository, PromoUserRepository>();

            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IInvoiceReposistory, InvoiceRepository>();

            services.AddScoped<ICommunityRepository, CommunityRepository>();
            services.AddScoped<ICommunityMemberRepository, CommunityMemberRepository>();

            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();

            // Infra - Identity Services
            services.AddTransient<IEmailSender, AuthEmailMessageSender>();
            services.AddTransient<ISmsSender, AuthSMSMessageSender>();

            // Infra - Identity
            services.AddScoped<IUser, User>();
            services.AddSingleton<IJwtFactory, JwtFactory>();

            // Infra - FileManager
            services.AddScoped<IFileManagerLogic, FileManagerLogic>();

        }
    }
}
