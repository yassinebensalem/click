using System;
using System.Linq;
using System.Linq.Expressions;
using DDD.Domain.Core.Models;
using DDD.Domain.Interfaces;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DDD.Infra.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(ApplicationDbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual TEntity Add(TEntity obj)
        {
            DbSet.Add(obj);
            return obj;
        }

        public virtual TEntity GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        public virtual TEntity GetById(int id)
        {
            return DbSet.Find(id);
        }

        //public virtual IQueryable<TEntity> GetAll()
        //{
        //    if (typeof(TEntity).IsSubclassOf(typeof(IEntityAudit)) || typeof(IEntityAudit).IsAssignableFrom(typeof(TEntity)))
        //    {
        //        return DbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
        //    }
        //    return DbSet;
        //}

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, bool includeIsDeleted = false)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(IEntityAudit)) || typeof(IEntityAudit).IsAssignableFrom(typeof(TEntity)))
            {
                if (predicate is null)
                {
                    return DbSet.Where(e => includeIsDeleted || EF.Property<bool>(e, "IsDeleted") == false);
                }
                return DbSet.Where(e => includeIsDeleted || EF.Property<bool>(e, "IsDeleted") == false).Where(predicate);
            }
            return DbSet;
        }

        public virtual IQueryable<TEntity> GetAll(ISpecification<TEntity> spec, Expression<Func<TEntity, bool>> predicate = null, bool includeIsDeleted = false)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(IEntityAudit)) || typeof(IEntityAudit).IsAssignableFrom(typeof(TEntity)))
            {
                if (predicate is null)
                {
                    return ApplySpecification(spec).Where(e => includeIsDeleted || EF.Property<bool>(e, "IsDeleted") == false);
                }
                return ApplySpecification(spec).Where(e => includeIsDeleted || EF.Property<bool>(e, "IsDeleted") == false).Where(predicate);
            }
            return ApplySpecification(spec).Where(e => includeIsDeleted || EF.Property<bool>(e, "IsDeleted") == false);
        }

        public virtual IQueryable<TEntity> GetAllSoftDeleted()
        {
            return DbSet.IgnoreQueryFilters()
                .Where(e => EF.Property<bool>(e, "IsDeleted") == true);
        }

        public virtual void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public virtual void Remove(Guid id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(DbSet.AsQueryable(), spec);
        }
    }
}
