using Asakabank.Base;

namespace Asakabank.UserApi.Entities {
    public class DbBlog : BaseEntity {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}