using System;
using System.Linq;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
namespace DDD.Infra.Data.Repository
{
    public class WalletTransactionRepository : Repository<WalletTransaction>, IWalletTransactionRepository
    {
        public WalletTransactionRepository(ApplicationDbContext context)
            : base(context)
        {

        }
                
    }
}
