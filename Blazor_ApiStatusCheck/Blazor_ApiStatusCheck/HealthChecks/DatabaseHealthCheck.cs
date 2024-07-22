using Blazor_ApiStatusCheck.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blazor_ApiStatusCheck.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseHealthCheck(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Burada veritabanı bağlantısını test eden kodları yazın
                var isConnected = await _dbContext.Database.CanConnectAsync(cancellationToken);
               
                if (isConnected)
                {
                    return HealthCheckResult.Healthy("Veritabanı bağlantısı başarılı.");
                }
                else
                {
                    return HealthCheckResult.Unhealthy("Veritabanı bağlantısı sağlanamadı.");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Degraded("Veritabanı bağlantısı sırasında bir hata oluştu.", ex);
            }
        }
    }
}
