using System;
using System.Collections.Generic;
using System.Text;

namespace DevJourney.Domain.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
    public bool IsDeleted { get; set; } = false;

    protected BaseEntity()
    {
    }

    protected BaseEntity(Guid id)
    {
        Id = id;
    }
}