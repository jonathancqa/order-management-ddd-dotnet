using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared;

public class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; }
    public DateTime? UpdatedDate { get; protected set; }
    public bool IsDeleted { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void MarkAsDelete()
    {
        IsDeleted = true;
        UpdatedDate = DateTime.UtcNow;
    }

    protected void SetUpdated()
    {
        UpdatedDate = DateTime.UtcNow;
    }
}