using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace Blazor_ApiStatusCheck.HealthChecks.Performance_Resources_HC
{
    public class DiskSpaceHealthCheck : IHealthCheck
    {
        private readonly string _driveName;
        private readonly long _minimumFreeMegabytes;

        public DiskSpaceHealthCheck(string driveName, long minimumFreeMegabytes)
        {
            _driveName = driveName ?? throw new ArgumentNullException(nameof(driveName));
            _minimumFreeMegabytes = minimumFreeMegabytes;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                DriveInfo drive = new DriveInfo(_driveName);

                if (!drive.IsReady)
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy($"Drive {_driveName} is not ready."));
                };
                long freeMegabytes = drive.AvailableFreeSpace / (1024 * 1024);
                if (freeMegabytes < _minimumFreeMegabytes)
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy(
                    $"Drive {_driveName} has only {freeMegabytes} MB free space,which is less than minimum required {_minimumFreeMegabytes} MB."
                        ));
                };
                return Task.FromResult(HealthCheckResult.Healthy(
                $"Drive {_driveName} has {freeMegabytes} MB free space."));

            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    $"Exception during disk check for {_driveName}: {ex.Message}"));
            }
        }
    }
}
