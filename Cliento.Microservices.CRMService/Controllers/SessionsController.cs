using Cliento.Microservices.CRMService.Cache;
using Cliento.Microservices.CRMService.Data;
using Cliento.Microservices.CRMService.Extensions;
using Cliento.Microservices.CRMService.Models;
using Cliento.Microservices.Shared.Events;
using Cliento.Microservices.Shared.Messaging;
using Microsoft.AspNetCore.Mvc;


namespace Cliento.Microservices.CRMService.Controllers
{
    [ApiController]
    [Route("api/clients/{clientId}/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly CrmDbContext _db;
        private readonly RedisSessionCache _cache;
        private readonly IEventPublisher _publisher;

        public SessionsController(CrmDbContext db, RedisSessionCache cache, IEventPublisher publisher)
        {
            _db = db;
            _cache = cache;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid clientId)
        {
            if (!HttpContext.TryGetUserId(out var userId)) return Unauthorized();

            var client = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);
            if (client == null) return NotFound();

            var cached = await _cache.GetSessionsAsync(clientId);
            if (cached != null) return Ok(cached);

            var sessions = await _db.Sessions.Where(s => s.ClientId == clientId).ToListAsync();
            await _cache.SetSessionsAsync(clientId, sessions);
            return Ok(sessions);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid clientId, [FromBody] CreateSessionDto dto)
        {
            if (!HttpContext.TryGetUserId(out var userId)) return Unauthorized();

            var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);
            if (client == null) return NotFound();

            var session = new Session
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                ScheduledAt = dto.ScheduledAt,
                DurationInMinutes = dto.DurationInMinutes,
                Notes = dto.Notes,
                Status = SessionStatus.Planned
            };
            _db.Sessions.Add(session);
            await _db.SaveChangesAsync();

            // invalidate cache for this client
            await _cache.InvalidateAsync(clientId);

            // Publish event
            var evt = new SessionPlannedEvent(session.Id, session.ClientId, session.ScheduledAt, session.DurationInMinutes);
            await _publisher.PublishAsync("session.planned", evt);

            return CreatedAtAction(nameof(GetById), new { clientId = clientId, id = session.Id }, session);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid clientId, Guid id)
        {
            if (!HttpContext.TryGetUserId(out var userId)) return Unauthorized();
            var s = await _db.Sessions.Include(x => x.Client).FirstOrDefaultAsync(x => x.Id == id && x.ClientId == clientId && x.Client!.UserId == userId);
            if (s == null) return NotFound();
            return Ok(s);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid clientId, Guid id, [FromBody] UpdateSessionDto dto)
        {
            if (!HttpContext.TryGetUserId(out var userId)) return Unauthorized();
            var session = await _db.Sessions.Include(x => x.Client).FirstOrDefaultAsync(x => x.Id == id && x.ClientId == clientId && x.Client!.UserId == userId);
            if (session == null) return NotFound();

            session.ScheduledAt = dto.ScheduledAt;
            session.DurationInMinutes = dto.DurationInMinutes;
            session.Notes = dto.Notes;
            session.Status = dto.Status;

            await _db.SaveChangesAsync();
            await _cache.InvalidateAsync(clientId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid clientId, Guid id)
        {
            if (!HttpContext.TryGetUserId(out var userId)) return Unauthorized();
            var session = await _db.Sessions.Include(x => x.Client).FirstOrDefaultAsync(x => x.Id == id && x.ClientId == clientId && x.Client!.UserId == userId);
            if (session == null) return NotFound();

            _db.Sessions.Remove(session);
            await _db.SaveChangesAsync();
            await _cache.InvalidateAsync(clientId);
            return NoContent();
        }
    }
}
