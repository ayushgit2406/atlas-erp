using System.Text.Json;
using StackExchange.Redis;

namespace OrderService.Infrastructure.Idempotency;

public class RedisIdempotencyStore : IIdempotencyStore
{
    private readonly IDatabase _db;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private record IdempotencyRecord(string RequestHash, int StatusCode, string ResponseBody);

    public RedisIdempotencyStore(IConnectionMultiplexer mux)
    {
        _db = mux.GetDatabase();
    }

    public async Task<IdempotencyResult> TryGetAsync(string key, string requestHash, CancellationToken ct)
    {
        var redisKey = BuildKey(key);
        var value = await _db.StringGetAsync(redisKey);
        if (!value.HasValue)
        {
            return new IdempotencyResult(IdempotencyResultKind.Miss, null);
        }

        var record = JsonSerializer.Deserialize<IdempotencyRecord>(value!, JsonOptions);
        if (record == null)
        {
            return new IdempotencyResult(IdempotencyResultKind.Miss, null);
        }

        if (!string.Equals(record.RequestHash, requestHash, StringComparison.Ordinal))
        {
            return new IdempotencyResult(IdempotencyResultKind.Conflict, null);
        }

        return new IdempotencyResult(
            IdempotencyResultKind.Hit,
            new IdempotencyHit(record.RequestHash, record.StatusCode, record.ResponseBody));
    }

    public async Task SaveAsync(string key, string requestHash, int statusCode, string responseBody, TimeSpan ttl, CancellationToken ct)
    {
        var record = new IdempotencyRecord(requestHash, statusCode, responseBody);
        var payload = JsonSerializer.Serialize(record, JsonOptions);
        await _db.StringSetAsync(BuildKey(key), payload, ttl);
    }

    private static string BuildKey(string key) => $"idem:{key}";
}
