using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderManagement.Application.Orders.UseCases.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("The order must contain at least one item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(200);
            item.RuleFor(i => i.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Unit price must be greater than zero.");
            item.RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(1000);
        });
    }
}