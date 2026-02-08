using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Contracts;

namespace OrderService.Controllers;

[ApiController]
[Route("v1/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IValidator<CreateOrderRequest> _validator;

    public OrdersController(IOrderService orderService, IValidator<CreateOrderRequest> validator)
    {
        _orderService = orderService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateOrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var result = await _orderService.CreateOrderAsync(request);
        return Created($"/v1/orders/{result.OrderId}", result);
    }
}
