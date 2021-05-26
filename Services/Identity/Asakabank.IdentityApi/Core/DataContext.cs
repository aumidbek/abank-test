using System.Linq;
using System.Threading.Tasks;
using Asakabank.IdentityApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asakabank.IdentityApi.Core {
    public class DataContext : DbContext {
        public DbSet<DbUserToken> UserTokens { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) {
        }

        public async Task<int> SaveChangesAsync() {
            return await base.SaveChangesAsync();
        }

        public DbSet<T> DbSet<T>() where T : class {
            return Set<T>();
        }

        public new IQueryable<T> Query<T>() where T : class {
            return Set<T>();
        }
    }
}