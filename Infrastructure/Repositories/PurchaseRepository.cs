
using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IPurchaseRepository
{
    List<OrderItem> List(int userId);
    OrderItem Create(OrderItem purchase);
}

public class PurchaseRepository : IPurchaseRepository
{
    private static List<OrderItem> _purchases = new List<OrderItem>
    {
        new OrderItem
        {
            Id = 1,
            products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 200 }
            },
            Date = new DateTime(2024, 1, 1),
            TotalPrice = 300
        }
    };

    public List<OrderItem> List(int userId)
    {
        return _purchases.FindAll(p => p.Id == userId);
    }

    public OrderItem Create(OrderItem purchase)
    {
        _purchases.Add(purchase);
        return purchase;
    }
}
