
using System;
using System.Security.Claims;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebCommerce.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]

public class PurchaseController : ControllerBase
{
    private readonly IPurchasesService _purchaseService;

    public PurchaseController(IPurchasesService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public IActionResult List()
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var purchases = _purchaseService.List(userId);
        return Ok(purchases);
    }

    [HttpPost]
    public IActionResult Create(OrderItem purchase)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        purchase.Id = userId;
        var createdPurchase = _purchaseService.Create(purchase);
        return Ok(createdPurchase);
    }

}
