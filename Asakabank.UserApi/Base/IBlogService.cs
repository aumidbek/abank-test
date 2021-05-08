using System;
using System.Threading.Tasks;
using Asakabank.UserApi.Models;

namespace Asakabank.UserApi.Base {
    public interface IBlogService {
        Task<Guid> Create(Blog blog);
        Task<Blog> Get(Guid id);
        Task Update(Blog blog);
        Task Delete(Guid blogId);
    }
}