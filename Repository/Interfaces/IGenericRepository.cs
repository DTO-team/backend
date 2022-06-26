using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get List of TEntity
        /// </summary>
        /// <param name="filter">Represent for condidtion when get list from database</param>
        /// <param name="orderBy">Order List by condition</param>
        /// <param name="includedProperties">Implement eager loading with particular properties</param>
        /// <returns></returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includedProperties = "", int page = 0, int limit = 0);

        /// <summary>
        /// Return TEntity base on ID
        /// </summary>
        /// <param name="id">Id of TEntity need to be found</param>
        /// <returns></returns>
        TEntity GetById(Guid id);

        /// <summary>
        /// Insert new TEntity into database
        /// </summary>
        /// <param name="newEntity">new TEntity need to be inserted</param>
        void Insert(TEntity newEntity);

        /// <summary>
        /// Delete TEntity by Id in database
        /// </summary>
        /// <param name="id">Id of deleted TEntity</param>
        void DeleteById(Guid id);

        /// <summary>
        /// Delete TEntity 
        /// </summary>
        /// <param name="deletedEntity">TEntity need to be deleted</param>
        void Delete(TEntity deletedEntity);

        /// <summary>
        /// Update TEntity
        /// </summary>
        /// <param name="updatedEntity">TEntity need to be updated</param>
        void Update(TEntity updatedEntity);

        void InsertRange(IEnumerable<TEntity> newEntities);
    }
}
