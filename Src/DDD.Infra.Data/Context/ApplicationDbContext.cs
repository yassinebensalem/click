using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;
using DDD.Domain.Models;
using DDD.Domain.Services;
using DDD.Infra.Data.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DDD.Infra.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ICurrentUserService _currentUserService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<JoinRequest>().OwnsOne(
        //    order => order.DesiredBooks, ownedNavigationBuilder =>
        //    {
        //        ownedNavigationBuilder.ToJson();
        //        ownedNavigationBuilder.OwnsMany(si => si.Deliveries);
        //    });
        //}
        //public DbSet<Customer> Customers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Invoice> Invoices { get; set; } 
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<FavoriteBook> FavoriteBooks { get; set; }
        public DbSet<FavoriteCategory> FavoriteCategories { get; set; }
        public DbSet<Library> Libraries { get; set; }
        //public DbSet<Metrics> Metrics { get; set; }
        //public DbSet<SavedPage> SavedPages { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }

        public DbSet<Prize> Prizes { get; set; }
        public DbSet<PrizeBook> PrizeBooks { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionBook> PromotionBooks { get; set; }
        public DbSet<PromoUser> PromoUsers { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityMember> CommunityMembers { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new CustomerMap());

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CommunityMember>()
                .HasKey(m => new { m.CommunityId, m.MemberId });
        }

        // public override int SaveChanges()
        // {
        //     OnBeforeSaving();
        //     return base.SaveChanges();
        // }

        // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        // {
        //     OnBeforeSaving();
        //     return await base.SaveChangesAsync(cancellationToken);
        // }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is IEntityAudit || x.Entity is RelationshipEntityAudit)
                .ToList();
            UpdateSoftDelete(entities);
            UpdateTimestamps(entities);
        }

        private void UpdateSoftDelete(List<EntityEntry> entries)
        {
            var filtered = entries
                .Where(x => x.State == EntityState.Added
                    || x.State == EntityState.Deleted);

            foreach (var entry in filtered)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //entry.CurrentValues["IsDeleted"] = false;
                        ((IEntityAudit)entry.Entity).IsDeleted = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        //entry.CurrentValues["IsDeleted"] = true;
                        ((IEntityAudit)entry.Entity).IsDeleted = true;
                        break;
                }
            }
        }

        private void UpdateTimestamps(List<EntityEntry> entries)
        {
            var filtered = entries
                .Where(x => x.State == EntityState.Added
                    || x.State == EntityState.Modified);

            // TODO: Get real current user id
            var currentUserId = _currentUserService.UserId;

            foreach (var entry in filtered)
            {
                if (entry.State == EntityState.Added)
                {
                    ((IEntityAudit)entry.Entity).CreatedAt = DateTime.UtcNow;
                    ((IEntityAudit)entry.Entity).CreatedBy = currentUserId;
                }

                ((IEntityAudit)entry.Entity).UpdatedAt = DateTime.UtcNow;
                ((IEntityAudit)entry.Entity).UpdatedBy = currentUserId;
            }
        }
    }
}
