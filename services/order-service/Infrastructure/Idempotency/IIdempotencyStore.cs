namespace OrderService.Infrastructure.Idempotency;

public record IdempotencyHit(string RequestHash, int StatusCode, string ResponseBody);

public enum IdempotencyResultKind
{
    Miss,
    Hit,
    Conflict
}

public record IdempotencyResult(IdempotencyResultKind Kind, IdempotencyHit? Hit);

public interface IIdempotencyStore
{
    Task<IdempotencyResult> TryGetAsync(string key, string requestHash, CancellationToken ct);
    Task SaveAsync(string key, string requestHash, int statusCode, string responseBody, TimeSpan ttl, CancellationToken ct);
}
