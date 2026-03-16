using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Repositories;
using OrderManagement.Domain.ValueObjects;
using FluentValidation;

namespace OrderManagement.Application.Orders.UseCases.CreateOrder;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _repository;
    private readonly CreateOrderValidator _validator;

    public CreateOrderUseCase(IOrderRepository repository, CreateOrderValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<CreateOrderResponse> ExecuteAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid) throw new ValidationException(result.Errors);

        var order = new Order();
        foreach (var item in request.Items)
        {
            order.AddItem(new OrderItem
            (
                item.ProductName,
                new Money(item.UnitPrice),
                item.Quantity
            ));
        }
        order.Create();

        await _repository.AddAsync(order, cancellationToken);

        return new CreateOrderResponse(
            order.Id, order.Status.ToString(),
            order.Total.Amount, order.CreatedDate);        
    }
}