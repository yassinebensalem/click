using AutoMapper;
using DDD.Application.ViewModels;
using DDD.Domain.Commands;
using DDD.Domain.Common.JSONColumns;
using DDD.Domain.Models;

namespace DDD.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CustomerViewModel, RegisterNewCustomerCommand>()
                .ConstructUsing(c => new RegisterNewCustomerCommand(c.Name, c.Email, c.BirthDate));
            CreateMap<CustomerViewModel, UpdateCustomerCommand>()
                .ConstructUsing(c => new UpdateCustomerCommand(c.Id, c.Name, c.Email, c.BirthDate));

            CreateMap<BookViewModel, RegisterNewBookCommand>()
                .ConstructUsing(b => new RegisterNewBookCommand(b.Id, b.Title, b.PageNumbers, b.CoverPath, b.Price, b.Description,
            b.PublicationDate, b.AuthorId, b.PublisherId, b.CategoryId, b.CountryId, b.LanguageId, b.ISBN, b.ISSN, b.EISBN, b.PDFPath ,b.Status));
            CreateMap<BookViewModel, UpdateBookCommand>()
            .ConstructUsing(b => new UpdateBookCommand(b.Id, b.Title, b.PageNumbers, b.CoverPath, b.Price, b.Description,
            b.PublicationDate, b.AuthorId, b.PublisherId, b.CategoryId, b.CountryId, b.LanguageId, b.ISBN, b.ISSN, b.EISBN, b.PDFPath , b.Status));

            CreateMap<BookStatePutVM, UpdateBookStateCommand>()
           .ConstructUsing(b => new UpdateBookStateCommand(b.Id, b.Status));

            CreateMap<CategoryViewModel, RegisterNewCategoryCommand>()
              .ConstructUsing(c => new RegisterNewCategoryCommand(c.CategoryName, c.Status,c.ParentId));
            CreateMap<CategoryViewModel, UpdateCategoryCommand>()
           .ConstructUsing(c => new UpdateCategoryCommand(c.Id, c.CategoryName, c.Status, c.ParentId));

            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src._LanguageId))
                 .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src._CountryId));


            CreateMap<AuthorVM, RegisterNewAuthorCommand>()
              .ConstructUsing(a => new RegisterNewAuthorCommand(a.Id,a.FirstName, a.LastName, a.Email, a.PhoneNumber , a.CountryId, a.Biography, a.Birthdate, a.PhotoPath , a.UserId)) ;
            CreateMap<AuthorVM, UpdateAuthorCommand>()
              .ConstructUsing(a => new UpdateAuthorCommand(a.Id, a.FirstName, a.LastName, a.Email, a.PhoneNumber, a.CountryId, a.Biography, a.Birthdate, a.PhotoPath, a.UserId));
            CreateMap<JoinRequestVM, RegisterNewJoinRequestCommand>()
                .ConstructUsing(b => new RegisterNewJoinRequestCommand(b.Id, b.LastName, b.FirstName, b.Email, b.Description , b.PhoneNumber, b.CountryId, b.RequesterType, b.Status, b.RaisonSocial, b.IdFiscal,b.VoucherNumber,b.VoucherValue,b.ReceiverEmail));
            CreateMap<JoinRequestVM, UpdateJoinRequestCommand>()
                .ConstructUsing(b => new UpdateJoinRequestCommand(b.Id, b.LastName, b.FirstName, b.Email, b.Description, b.PhoneNumber, b.CountryId, b.RequesterType, b.Status, b.RaisonSocial, b.IdFiscal));

            CreateMap<FavoriteBookVM, RegisterNewFavoriteBookCommand>()
              .ConstructUsing(c => new RegisterNewFavoriteBookCommand(c.UserId, c.BookId));

            CreateMap<FavoriteCategoryVM, RegisterNewFavoriteCategoryCommand>()
              .ConstructUsing(c => new RegisterNewFavoriteCategoryCommand(c.UserId, c.CategoryId));

            CreateMap<CartVM, RegisterNewCartCommand>()
                         .ConstructUsing(c => new RegisterNewCartCommand(c.UserId, c.BookId));

            CreateMap<PrizeVM, RegisterNewPrizeCommand>()
                        .ConstructUsing(c => new RegisterNewPrizeCommand(c.Id,c.Name,c.Description,c.CountryId,c.WebSiteUrl, c.FacebookUrl ,c.LogoPath));

            CreateMap<PrizeVM, Prize>()
               .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.LogoPath));

            CreateMap<PrizeBookVM, RegisterNewPrizeBookCommand>()
                      .ConstructUsing(c => new RegisterNewPrizeBookCommand(c.Id,c.PrizeId,c.BookId,c.Edition));

            CreateMap<InvoiceVM, AddNewInvoiceCommand>()
                       .ConstructUsing(c => new AddNewInvoiceCommand(c.Id, c.UserId, c.BookId, c.Date,c.Amount,c.Price,c.OrderNumber, c.PaymentType, c.AuthorizationCode, c.PaymentReason));

            CreateMap<LibraryVM, RegisterNewLibraryCommand>()
                      .ConstructUsing(c => new RegisterNewLibraryCommand(c.Id, c.UserId, c.BookId));

            CreateMap<UpdateLibraryVM, UpdateLibraryCommand>()
                     .ConstructUsing(c => new UpdateLibraryCommand(c.Id, c.UserId, c.BookId,c.CurrentPage));

            CreateMap<PromotionVM, RegisterNewPromotionCommand>()
                        .ConstructUsing(c => new RegisterNewPromotionCommand(c.Name, c.PromotionType, c.Percentage, c.StartDate , c.EndDate, c.Description, c.ImagePath, c.CountryId));
             
            CreateMap<PromotionBookVM, RegisterNewPromotionBookCommand>()
                      .ConstructUsing(c => new RegisterNewPromotionBookCommand(c.Id, c.PromotionId, c.BookId));

            CreateMap<PromotionVM, UpdatePromotionCommand>()
                        .ConstructUsing(c => new UpdatePromotionCommand(c.Name, c.PromotionType, c.Percentage, c.StartDate, c.EndDate, c.Description, c.ImagePath, c.CountryId));

            CreateMap<PromoUserVM, RegisterNewPromoUserCommand>()
                      .ConstructUsing(c => new RegisterNewPromoUserCommand(c.UserId, c.PromotionId, c.BooksTakenCount));

            CreateMap<PromoUserVM, UpdatePromoUserCommand>()
                        .ConstructUsing(c => new UpdatePromoUserCommand(c.UserId, c.PromotionId, c.BooksTakenCount));

            CreateMap<BookCompetitionVM, BookCompetition>();

            CreateMap<CommunityEditViewModel, RegisterNewCommunityCommand>()
              .ConstructUsing(c => new RegisterNewCommunityCommand(c.CommunityName, c.AdminId, c.Status));
            CreateMap<CommunityEditViewModel, UpdateCommunityCommand>()
           .ConstructUsing(c => new UpdateCommunityCommand(c.Id, c.CommunityName, c.AdminId, c.Status));

            CreateMap<CommunityMemberViewModel, AssociateCommunityMemberCommand>()
              .ConstructUsing(c => new AssociateCommunityMemberCommand(c.CommunityId, c.MemberId, c.Status));
            CreateMap<CommunityMemberViewModel, DissociateCommunityMemberCommand>()
              .ConstructUsing(c => new DissociateCommunityMemberCommand(c.CommunityId, c.MemberId, c.Status));
            CreateMap<CommunityInvitationalViewModel, InviteToCommunityWithMembershipCommand>()
                          .ConstructUsing(c => new InviteToCommunityWithMembershipCommand(c.CommunityId, c.Email, c.IsCommunityAdmin, c.Status));

            CreateMap<WalletDispatchTransactionViewModel, RefillWalletTransactionCommand>()
              .ConstructUsing(c => new RefillWalletTransactionCommand(c.UserIds, c.CommunityId, c.Amount, c.Status, c.InvoiceId));
            CreateMap<WalletDispatchTransactionViewModel, WithdrawWalletTransactionCommand>()
              .ConstructUsing(c => new WithdrawWalletTransactionCommand(c.UserIds, c.CommunityId, c.Amount, c.Status));

        }
    }
}
