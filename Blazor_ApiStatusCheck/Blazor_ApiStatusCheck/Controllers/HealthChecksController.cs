using Blazor_ApiStatusCheck.Connection;
using Blazor_ApiStatusCheck.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyBlazorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HealthCheckController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/HealthCheck
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiHealthCheck>>> GetHealthChecks()
        {
            return await _context.HealthChecks.OrderByDescending(hc => hc.LastChecked).ToListAsync();
        }

        // GET: api/HealthCheck/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiHealthCheck>> GetHealthCheck(string id)
        {
            var healthCheck = await _context.HealthChecks.FindAsync(id);

            if (healthCheck == null)
            {
                return NotFound();
            }

            return healthCheck;
        }

        // POST: api/HealthCheck
        [HttpPost]
        public async Task<ActionResult<ApiHealthCheck>> PostHealthCheck(ApiHealthCheck apiHealthCheck)
        {
            _context.HealthChecks.Add(apiHealthCheck);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHealthCheck), new { id = apiHealthCheck.Api__Id }, apiHealthCheck);
        }

        // PUT: api/HealthCheck/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHealthCheck(string id, ApiHealthCheck apiHealthCheck)
        {
            if (id != apiHealthCheck.Api__Id)
            {
                return BadRequest();
            }

            _context.Entry(apiHealthCheck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApiHealthCheckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/HealthCheck/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthCheck(string id)
        {
            var healthCheck = await _context.HealthChecks.FindAsync(id);
            if (healthCheck == null)
            {
                return NotFound();
            }

            _context.HealthChecks.Remove(healthCheck);
            await _context.SaveChangesAsync();

            return Ok(healthCheck);//NoContent();
        }

        // Sağlık kontrolünü gerçekleştir ve sonuçları kaydet
        [HttpPost("CheckApis")]
        public async Task<IActionResult> CheckApis([FromBody] List<ApiHealthCheck> apisToCheck)
        {
            using var client = new HttpClient();

            foreach (var api in apisToCheck)
            {
                var response = await client.GetAsync(api.Url);
                var status = response.IsSuccessStatusCode ? "healthy" : "unhealthy";
                var message = response.IsSuccessStatusCode ? "API is healthy" : "API is unhealthy";

                var apiHealthCheck = new ApiHealthCheck
                {
                    Name = api.Name,
                    Url = api.Url,
                    LastChecked = DateTime.Now,
                    Status = status,
                    Message = message,
                    CreatedAt = api.CreatedAt != default ? api.CreatedAt : DateTime.Now
                };

                _context.HealthChecks.Add(apiHealthCheck);
             
            }

            await _context.SaveChangesAsync();

            return Ok("APIs health check completed");
        }

        private bool ApiHealthCheckExists(string id)
        {
            return _context.HealthChecks.Any(e => e.Api__Id == id);
        }

    }
}