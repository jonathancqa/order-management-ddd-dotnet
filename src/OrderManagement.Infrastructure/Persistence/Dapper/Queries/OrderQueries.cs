using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Persistence.Dapper.Queries;

public static class OrderQueries
{
    public const string GetById = @"
        SELECT
            o.Id,
            o.Status,
            o.CreatedAt,
            o.UpdatedAt,
            o.IsDeleted,
            i.Id AS ItemId,
            i.ProductName,
            i.UnitPrice,
            i.Quantity,
            i.CreatedAt AS ItemCreatedAt
        FROM tb_orders o
        LEFT JOIN tb_order_itens i ON i.OrderId = o.Id
        WHERE o.Id = @Id AND o.IsDeleted = 0";
    
    public const string GetAll = @"
        SELECT
            o.Id,
            o.Status,
            o.CreatedAt,
            COALESCE (SUM(i.UnitPrice * i.Quantity), 0) AS Total
        FROM tb_orders o
        LEFT JOIN tb_order_itens i ON i.OrderId = o.Id
        WHERE o.IsDeleted = 0
        GROUP BY o.Id, o.Status, o.CreatedAt
        ORDER BY o.CreatedAt DESC";

    public const string Exists = @"
        SELECT COUNT(1) FROM tb_orders
        WHERE Id = @Id AND IsDeleted = 0";
}