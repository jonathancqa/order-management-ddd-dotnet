using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Entities;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _items = new();

    public OrderStatus Status { get; private set; } = OrderStatus.Draft;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money Total => _items.Aggregate(new Money(0), (acc, item) => acc + item.Total);

    public void AddItem(OrderItem item)
    {
        if (Status != OrderStatus.Draft)
            throw new DomainException("Cannot add items to a non-draft order");
            
        _items.Add(item);
    }

    public void Create()
    {
        if (! _items.Any())
            throw new DomainException("Cannot create an order without items");
        
        Status = OrderStatus.Created;
        
        SetUpdated();
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created)
            throw new DomainException("Cannot mark as paid an order that is not created");

        Status = OrderStatus.Paid;

        SetUpdated();
    }

    public void Ship()
    {
        if (Status != OrderStatus.Paid)
            throw new DomainException("Cannot ship an order that is not paid");

        Status = OrderStatus.Shipped;

        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new DomainException("Cannot cancel an order that is already shipped");

        Status = OrderStatus.Cancelled;

        SetUpdated();
    }

    public Order() {}

    private Order(bool _) {}
}
