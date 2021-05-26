using System.Linq;
using System.Threading.Tasks;
using Asakabank.UserApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asakabank.UserApi.Core {
    public class DataContext : DbContext {
        public DbSet<DbUser> Users { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            #region Users

            modelBuilder.Entity<DbUser>().HasData(new DbUser(System.Guid.NewGuid()) {
                Username = "test1",
                Password = BCrypt.Net.BCrypt.HashPassword("password1")
            });
            modelBuilder.Entity<DbUser>().HasData(new DbUser(System.Guid.NewGuid()) {
                Username = "test2",
                Password = BCrypt.Net.BCrypt.HashPassword("password2")
            });

            #endregion
        }
    }
}