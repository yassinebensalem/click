using System;
using System.Linq;
using System.Linq.Expressions;

namespace DDD.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Add(TEntity obj);
        TEntity GetById(Guid id);
        TEntity GetById(int id);
        //IQueryable<TEntity> GetAll();
        //IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, bool includeIsDeleted = false);
        IQueryable<TEntity> GetAll(ISpecification<TEntity> spec, Expression<Func<TEntity, bool>> predicate = null, bool includeIsDeleted = false);
        IQueryable<TEntity> GetAllSoftDeleted();
        void Update(TEntity obj);
        void Remove(Guid id);
        int SaveChanges();
    }
}
