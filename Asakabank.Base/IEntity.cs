using System;

namespace Asakabank.Base {
    public interface IEntity {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
    }
}