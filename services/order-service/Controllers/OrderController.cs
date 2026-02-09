using System.Security.Cryptography;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Contracts;
using OrderService.Infrastructure.Idempotency;

namespace OrderService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IValidator<CreateOrderRequest> _validator;
    private readonly IIdempotencyStore _idempotencyStore;

    public OrdersController(
        IOrderService orderService,
        IValidator<CreateOrderRequest> validator,
        IIdempotencyStore idempotencyStore)
    {
        _orderService = orderService;
        _validator = validator;
        _idempotencyStore = idempotencyStore;
    }

    [HttpPost]
    public async Task<ActionResult<CreateOrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
        var correlationId = HttpContext.Request.Headers["X-Correlation-Id"].ToString();
        var idempotencyKey = HttpContext.Request.Headers["Idempotency-Key"].ToString();

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            var requestHash = ComputeRequestHash(request);
            var cached = await _idempotencyStore.TryGetAsync(idempotencyKey, requestHash, HttpContext.RequestAborted);
            if (cached.Kind == IdempotencyResultKind.Conflict)
            {
                return Conflict(new { message = "Idempotency-Key reuse with different request payload." });
            }

            if (cached.Kind == IdempotencyResultKind.Hit && cached.Hit != null)
            {
                var response = JsonSerializer.Deserialize<CreateOrderResponse>(cached.Hit.ResponseBody);
                if (response != null)
                {
                    return Created($"/v{version}/orders/{response.OrderId}", response);
                }
            }
        }

        var result = await _orderService.CreateOrderAsync(request, correlationId, idempotencyKey);
        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            var responseBody = JsonSerializer.Serialize(result);
            await _idempotencyStore.SaveAsync(
                idempotencyKey,
                ComputeRequestHash(request),
                StatusCodes.Status201Created,
                responseBody,
                TimeSpan.FromHours(24),
                HttpContext.RequestAborted);
        }

        return Created($"/v{version}/orders/{result.OrderId}", result);
    }

    [HttpPost("{orderId:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid orderId)
    {
        var correlationId = HttpContext.Request.Headers["X-Correlation-Id"].ToString();
        var idempotencyKey = HttpContext.Request.Headers["Idempotency-Key"].ToString();
        var updated = await _orderService.CancelOrderAsync(orderId, correlationId, idempotencyKey);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{orderId:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid orderId)
    {
        var correlationId = HttpContext.Request.Headers["X-Correlation-Id"].ToString();
        var idempotencyKey = HttpContext.Request.Headers["Idempotency-Key"].ToString();
        var updated = await _orderService.ConfirmOrderAsync(orderId, correlationId, idempotencyKey);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{orderId:guid}/fulfill")]
    public async Task<IActionResult> Fulfill(Guid orderId)
    {
        var correlationId = HttpContext.Request.Headers["X-Correlation-Id"].ToString();
        var idempotencyKey = HttpContext.Request.Headers["Idempotency-Key"].ToString();
        var updated = await _orderService.FulfillOrderAsync(orderId, correlationId, idempotencyKey);
        return updated ? NoContent() : NotFound();
    }

    private static string ComputeRequestHash(CreateOrderRequest request)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(request);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
