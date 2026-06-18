using Cliento.Microservices.CRMService.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Cliento.Microservices.CRMService.Cache
{
 
    public class RedisSessionCache
    {
        private readonly IDatabase _db;
        private const int MINUTES = 10;
        public RedisSessionCache(IConnectionMultiplexer mux)
        {
            _db = mux.GetDatabase();
        }

        private string Key(Guid clientId) => $"client:{clientId}:sessions";

        public async Task<List<Session>> GetSessionsAsync(Guid clientId)
        {
            var val = await _db.StringGetAsync(Key(clientId));
            if (!val.HasValue) return null;
            return JsonSerializer.Deserialize<List<Session>>(val!)!;
        }

        public async Task SetSessionsAsync(Guid clientId, List<Session> sessions, TimeSpan? ttl = null)
        {
            var json = JsonSerializer.Serialize(sessions);
            await _db.StringSetAsync(Key(clientId), json, ttl ?? TimeSpan.FromMinutes(MINUTES));
        }

        public async Task InvalidateAsync(Guid clientId)
        {
            await _db.KeyDeleteAsync(Key(clientId));
        }
    }
}
