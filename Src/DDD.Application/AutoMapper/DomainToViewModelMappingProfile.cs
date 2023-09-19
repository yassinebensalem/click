using System;
using AutoMapper;
using DDD.Application.ViewModels;
using DDD.Domain.Common.JSONColumns;
using DDD.Domain.Models;
using DDD.Domain.Specifications;

namespace DDD.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<Book, BookViewModel>();
            CreateMap<BookCompetition, BookCompetitionVM>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Author, AuthorVM>();
            CreateMap<Language, LanguageViewModel>();
            CreateMap<Country, CountryViewModel>();
            CreateMap<JoinRequest, JoinRequestVM>()
                 .ForMember(dest => dest.DesiredBooks, opt => opt.Ignore());
            CreateMap<FavoriteBook, FavoriteBookVM>();
            CreateMap<FavoriteCategory, FavoriteCategoryVM>();
            CreateMap<Cart, CartVM>();
            CreateMap<Prize, PrizeVM>()
                .ForMember(dest => dest.Logo, opt => opt.Ignore())
                .ForMember(dest => dest.LogoPath, opt => opt.MapFrom(src => src.Logo));
            CreateMap<PrizeBook, PrizeBookVM>();
            CreateMap<Library, LibraryVM>();
            CreateMap<Invoice, InvoiceVM>();
            CreateMap<Promotion, PromotionVM>();
            CreateMap<PromotionBook, PromotionBookVM>();
            CreateMap<PromoUser, PromoUserVM>();
            CreateMap<PagedSelledBooks, PagedSelledBooksVM>();
            CreateMap<SelledBooksChart, SelledBooksChartVM>();
            CreateMap<PagedBestPublishers, PagedBestPublishersVM>();
            CreateMap<PagedBestSubscribers, PagedBestSubscribersVM>();
            CreateMap<PagedSubscriberPurchase, PagedSubscriberPurchaseVM>();
            CreateMap<Community, CommunityViewModel>();
            CreateMap<ApplicationUser, ApplicationUserViewModel>();
            CreateMap<WalletTransaction, WalletTransactionViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ApplicationUserId)?Guid.Empty : Guid.Parse(src.ApplicationUserId)));
        }
    }
}
