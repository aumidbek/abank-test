using System;
using System.Threading.Tasks;
using Asakabank.Base;
using Asakabank.UserApi.Base;
using Asakabank.UserApi.Entities;
using Asakabank.UserApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Asakabank.UserApi.Services {
    public class BlogService : IBlogService {
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public BlogService(IDbRepository dbRepository, IMapper mapper) {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Create(Blog blog) {
            var entity = _mapper.Map<DbBlog>(blog);

            var result = await _dbRepository.Add(entity);
            await _dbRepository.SaveChangesAsync();

            return result;
        }

        public async Task<Blog> Get(Guid id) {
            var blog = await _dbRepository.Get<DbBlog>().FirstOrDefaultAsync(x => x.Id == id);
            var blogModel = _mapper.Map<Blog>(blog);
            await _dbRepository.SaveChangesAsync();
            return blogModel;
        }

        public async Task Update(Blog blog) {
            var entity = _mapper.Map<DbBlog>(blog);

            await _dbRepository.Update(entity);
            await _dbRepository.SaveChangesAsync();
        }

        public async Task Delete(Guid blogId) {
            await _dbRepository.Delete<DbBlog>(blogId);
            await _dbRepository.SaveChangesAsync();
        }
    }
}