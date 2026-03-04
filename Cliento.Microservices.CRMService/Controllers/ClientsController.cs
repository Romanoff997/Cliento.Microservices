using Cliento.Microservices.CRMService.Data;
using Cliento.Microservices.CRMService.DTOs;
using Cliento.Microservices.CRMService.Extensions;
using Cliento.Microservices.CRMService.Models;
using Cliento.Microservices.Shared.Events;
using Cliento.Microservices.Shared.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Cliento.Microservices.CRMService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly CrmDbContext _db;
        private readonly IEventPublisher _publisher;

        public ClientsController(CrmDbContext db, IEventPublisher publisher)
        {
            _db = db;
            _publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
        {
            if (!HttpContext.TryGetUserId(out var userId)) return Unauthorized();

            var client = new Client { Id = Guid.NewGuid(), Name = dto.Name, Email = dto.Email, Phone = dto.Phone, UserId = userId, CreatedAt = DateTime.UtcNow };
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            // Publish event
            var evt = new ClientCreatedEvent(client.Id, client.Name, client.Email);
            await _publisher.PublishAsync("client.created", evt);

            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!HttpContext.TryGetUserId(out var userId)) return Unauthorized();
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (client == null) return NotFound();
            return Ok(client);
        }

        // Update/Delete similarly — check UserId
    }
}
