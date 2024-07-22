namespace Blazor_ApiStatusCheck.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ApiHealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        // Burada sağlık denetimlerinin konfigürasyonlarını saklayacağımız bir yerel bellek (in-memory) liste
        private static List<CustomHealthCheck> _customHealthChecks = new List<CustomHealthCheck>();

        public ApiHealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            var status = report.Status == HealthStatus.Healthy ? "Healthy" : "Unhealthy";
            return Ok(new { status, report });
        }
        //[HttpGet("api1")]
        //public async Task<IActionResult> GetApi1Health()
        //{
        //    var report = await _healthCheckService.CheckHealthAsync();
        //    var status = report.Status == HealthStatus.Healthy ? "Healthy" : "Unhealthy";
        //    return Ok(new {api="api1" ,status, report });
        //}
        //[HttpGet("api2")]
        //public async Task<IActionResult> GetApi2Health()
        //{
        //    var report = await _healthCheckService.CheckHealthAsync();
        //    var status = report.Status == HealthStatus.Healthy ? "Healthy" : "Unhealthy";
        //    return Ok(new { api = "api2", status, report });
        //}

        //[HttpGet("api3")]
        //public async Task<IActionResult> GetApi3Health()
        //{
        //    var report = await _healthCheckService.CheckHealthAsync();
        //    var status = report.Status == HealthStatus.Healthy ? "Healthy" : "Unhealthy";
        //    return Ok(new { api = "api3", status, report });
        //}

        [HttpPost]
        public IActionResult Post([FromBody] CustomHealthCheck healthCheck)
        {
            // Yeni sağlık denetimi ekleme
            _customHealthChecks.Add(healthCheck);
            return Ok(new { message = "Health check added", healthCheck });
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            // Belirtilen isimdeki sağlık denetimini bulma ve silme
            var healthCheck = _customHealthChecks.Find(hc => hc.Name == name);
            if (healthCheck != null)
            {
                _customHealthChecks.Remove(healthCheck);
                return Ok(new { message = "Health check removed", healthCheck });
            }
            return NotFound(new { message = "Health check not found" });
        }
    }

    // Örnek olarak kullanılan özel sağlık denetimi sınıfı
    public class CustomHealthCheck
    {
        public required string Name { get; set; }
        public bool IsHealthy { get; set; }
        // Diğer gerekli sağlık denetimi özellikleri
    }
}
