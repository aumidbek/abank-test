using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Asakabank.Base;
using Microsoft.EntityFrameworkCore;

namespace Asakabank.IdentityApi.Core {
    public class DbRepository : IDbRepository {
        private readonly DataContext _context;

        public DbRepository(DataContext context) {
            _context = context;
        }

        public IQueryable<T> Get<T>() where T : class, IEntity {
            return _context.Set<T>().Where(x => !x.IsDeleted).AsQueryable();
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IEntity {
            return _context.Set<T>().Where(selector).Where(x => !x.IsDeleted).AsQueryable();
        }

        public async Task<Guid> Add<T>(T newEntity) where T : class, IEntity {
            var entity = await _context.Set<T>().AddAsync(newEntity);
            return entity.Entity.Id;
        }

        public async Task AddRange<T>(IEnumerable<T> newEntities) where T : class, IEntity {
            await _context.Set<T>().AddRangeAsync(newEntities);
        }

        public async Task Delete<T>(Guid id) where T : class, IEntity {
            var activeEntity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            activeEntity.IsDeleted = true;
            await Task.Run(() => _context.Update(activeEntity));
        }

        public async Task Remove<T>(T entity) where T : class, IEntity {
            await Task.Run(() => _context.Set<T>().Remove(entity));
        }

        public async Task RemoveRange<T>(IEnumerable<T> entities) where T : class, IEntity {
            await Task.Run(() => _context.Set<T>().RemoveRange(entities));
        }

        public async Task Update<T>(T entity) where T : class, IEntity {
            entity.UpdatedAt = DateTime.Now;
            await Task.Run(() => _context.Set<T>().Update(entity));
        }

        public async Task UpdateRange<T>(IEnumerable<T> entities) where T : class, IEntity {
            entities.ToList().ForEach(s => s.UpdatedAt = DateTime.Now);
            await Task.Run(() => _context.Set<T>().UpdateRange(entities));
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>() where T : class, IEntity {
            return _context.Set<T>().AsQueryable();
        }
    }
}