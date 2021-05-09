using System;

namespace Asakabank.Base {
    public class BaseEntity : IEntity {
        protected BaseEntity(Guid id) {
            Id = id;
        }

        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
    }
}