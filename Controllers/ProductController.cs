
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebCommerce.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize (Roles = "Admin")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult List()
    {
        var products = _productService.List();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult GetById(int id)
    {
        var product = _productService.GetById(id);
        return Ok(product);
    }

    [HttpPost]

    public IActionResult Create(Product product)
    {
        var createdProduct = _productService.Create(product);
        return Ok(createdProduct);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Product product)
    {
        product.Id = id;

        var updatedProduct = _productService.Update(product);
        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _productService.Delete(id);
        return NoContent();
    }
}


