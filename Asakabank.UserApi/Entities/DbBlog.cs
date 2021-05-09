using System;
using Asakabank.Base;

namespace Asakabank.UserApi.Entities {
    public class DbBlog : BaseEntity {
        protected DbBlog(Guid id) : base(id) {
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}