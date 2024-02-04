
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Validators;
using Infrastructure.Repositories;

namespace Application.Services;

public interface IPurchasesService
{
    List<OrderItem> List(int userId);
    OrderItem Create(OrderItem purchase);
}

public class PurchasesService : IPurchasesService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IValidate<OrderItem> _purchaseValidator;

    public PurchasesService(IPurchaseRepository purchaseRepository, IValidate<OrderItem> purchaseValidator)
    {
        _purchaseRepository = purchaseRepository;
        _purchaseValidator = purchaseValidator;
    }

    public List<OrderItem> List(int userId)
    {
        return _purchaseRepository.List(userId);
    }

    public OrderItem Create(OrderItem purchase)
    {
        var errorMessages = _purchaseValidator.Validate(purchase);
        if (errorMessages.Any())
        {
            throw new BadRequestException(errorMessages);
        }
        return _purchaseRepository.Create(purchase);
    }
}
