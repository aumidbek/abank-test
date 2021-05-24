﻿using System;

namespace Asakabank.Base {
    public interface IEntity {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}