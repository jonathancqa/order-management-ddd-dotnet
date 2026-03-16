using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Application.Orders.UseCases.CreateOrder
{
    public record CreateOrderRequest
    {
        public List<OrderItemRequest> Items { get; init; } = new();
    }
    public record OrderItemRequest (string ProductName, decimal UnitPrice, int Quantity);
}