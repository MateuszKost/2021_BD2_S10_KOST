using SmartCollection.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.DataAccess.RepositoryPattern
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly SmartCollectionDbContext _db;

        public Repository(SmartCollectionDbContext db)
        {
            _db = db;
        }
        public async void AddAsync(TEntity entity)
        {
            await _db.AddAsync(entity);
        }

        public async void AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        public async void ReloadAsync(TEntity entity)
        {
            await _db.Entry(entity).GetDatabaseValuesAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Where(predicate);
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _db.Set<TEntity>().FindAsync(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _db.Set<TEntity>().ToList();
        }

        public void Remove(TEntity entity)
        {
            _db.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _db.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _db.Update(entity);
        }
    }
}
