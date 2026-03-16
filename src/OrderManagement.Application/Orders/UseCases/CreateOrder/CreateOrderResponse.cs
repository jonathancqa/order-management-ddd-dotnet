using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Application.Orders.UseCases.CreateOrder;

public record CreateOrderResponse(
    Guid Id,
    string Status,
    decimal Total,
    DateTime CreatedAt);