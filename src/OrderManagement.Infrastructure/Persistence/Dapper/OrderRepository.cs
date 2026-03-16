using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.Repositories;
using OrderManagement.Domain.ValueObjects;
using OrderManagement.Infrastructure.Persistence.Dapper.Queries;
using OrderManagement.Infrastructure.Persistence.EfCore;

namespace OrderManagement.Infrastructure.Persistence.Dapper;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _connection;

    public OrderRepository(AppDbContext context, IDbConnection connection)
    {
        _context = context;
        _connection = connection;
    }

    #region [Writes com EF Core]
    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken)
            ?? throw new NotFoundException($"Order {id} não encontrado.");
        order.MarkAsDelete();
        await _context.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region [Reads com Dapper]
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var orderDict = new Dictionary<Guid, Order>();

        await _connection.QueryAsync<Order, OrderItemRow, Order>(
            OrderQueries.GetById,
            (order, itemRow) =>
            {
                if (!orderDict.TryGetValue(order.Id, out var existing))
                {
                    existing = order;
                    orderDict[order.Id] = existing;
                }
                if (itemRow?.ItemId != null)
                {
                    var item = new OrderItem(
                        itemRow.ProductName,
                        new Money(itemRow.UnitPrice),
                        itemRow.Quantity);
                    existing.AddItem(item);
                }
                return existing;
            },
            param: new { Id = id },
            splitOn: "ItemId"
        );

        return orderDict.Values.FirstOrDefault();
    }

    public async Task<IReadOnlyList<OrderSummary>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _connection.QueryAsync<OrderSummary>(OrderQueries.GetAll);
        return result.ToList().AsReadOnly();
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var count = await _connection.ExecuteScalarAsync<int>(
            OrderQueries.Exists, new { Id = id });
        return count > 0;
    }
    #endregion

    // DTO interno para multi-mapping do Dapper
    internal record OrderItemRow(
        Guid ItemId,
        string ProductName,
        decimal UnitPrice,
        int Quantity
    );
}