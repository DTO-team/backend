﻿using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected CAPSTONEONGOINGContext context;
        protected DbSet<TEntity> dbSet;

        public GenericRepository(CAPSTONEONGOINGContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public void Delete(TEntity deletedEntity)
        {
           if(context.Entry(deletedEntity).State == EntityState.Detached)
            {
                dbSet.Attach(deletedEntity);
            }
           dbSet.Remove(deletedEntity);
        }

        public void DeleteById(Guid id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includedProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            foreach (string includeProperty in includedProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }

        }

        public TEntity GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public void Insert(TEntity newEntity)
        {
            dbSet.Add(newEntity);
        }

        public void Update(TEntity updatedEntity)
        {
            dbSet.Attach(updatedEntity);
            context.Entry(updatedEntity).State = EntityState.Modified;
        }
    }
}