using System;

namespace Asakabank.Base {
    public class BaseEntity : IEntity {
        protected BaseEntity(Guid id) {
            Id = id;
        }

        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}