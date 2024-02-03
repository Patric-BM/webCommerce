
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;

namespace Infrastructure.Repositories;

public interface IProductRepository
{
    Product GetProductById(int id);
    Product CreateProduct(Product product);
    Product UpdateProduct(Product product);
    void DeleteProduct(int id);
}

public class ProductRepository : IProductRepository
{
    private static List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Price = 100 },
        new Product { Id = 2, Name = "Product 2", Price = 200 }
    };

    public Product GetProductById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id) ?? throw new NotFoundException("Product not found");
    }

    public Product CreateProduct(Product product)
    {
        product.Id = _products.Max(p => p.Id) + 1;
        _products.Add(product);
        return product;
    }

    public Product UpdateProduct(Product product)
    {
        var existingProduct = GetProductById(product.Id);

        if (existingProduct is null)
        {
            throw new NotFoundException("Product not found");
        }

        _products[_products.IndexOf(existingProduct)] = product;
        return product;
    }

    public void DeleteProduct(int id)
    {
        var existingProduct = GetProductById(id);
        if (existingProduct is null)
        {
            throw new NotFoundException("Product not found");
        }
        _products.Remove(existingProduct);
    }
}
