
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Validators;
using Infrastructure.Repositories;

namespace Application.Services;

public interface IProductService
{
    Product Create(Product product);
    void Delete(int id);
    Product GetById(int id);
    Product Update(Product product);
    List<Product> List();
}

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidate<Product> _productValidator;

    public ProductService(IProductRepository productRepository, IValidate<Product> productValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public Product Create(Product product)
    {
        var _errorMessages = _productValidator.Validate(product);
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }
        _productRepository.CreateProduct(product);
        return product;
    }

    public void Delete(int id)
    {
        var _errorMessages = _productValidator.Validate(_productRepository.GetProductById(id));
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }
        _productRepository.DeleteProduct(id);
    }

    public Product GetById(int id)
    {
       
        return _productRepository.GetProductById(id);
    }

    public List<Product> List()
    {
        return _productRepository.List();
    }

    public Product Update(Product product)
    {
        var _errorMessages = _productValidator.Validate(product);
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }

        _productRepository.UpdateProduct(product);
        return product;
    }
}