using System;

namespace Asakabank.UserApi.Models {
    public class Blog {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}