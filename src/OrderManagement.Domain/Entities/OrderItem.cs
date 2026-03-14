using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Entities;

public class OrderItem : BaseEntity
{
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public Money Total => UnitPrice.Multiply(Quantity);

    public OrderItem(string productName, Money unitPrice, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product name id required.");

        if (unitPrice.Amount <= 0)
            throw new DomainException("Unit price must be greater than zero.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
    
    private OrderItem()
    {
        ProductName = null!;
        UnitPrice = null!;
    }
}