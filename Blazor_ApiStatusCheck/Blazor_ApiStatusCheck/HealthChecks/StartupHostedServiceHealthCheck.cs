using Blazor_ApiStatusCheck.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blazor_ApiStatusCheck.HealthChecks
{
    public class StartupHostedServiceHealthCheck : IHealthCheck
    {
        private volatile bool _startupTaskCompleted = false;

        public string Name => "slow_dependency_check";

        public bool StartupTaskCompleted
        {
            get => _startupTaskCompleted;
            set => _startupTaskCompleted = value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (StartupTaskCompleted)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("The startup task is finished."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("The startup task is still running."));
        }
    }
}

public class StartupHostedService : IHostedService, IDisposable
{
    private readonly int _delaySeconds = 15;
    private readonly ILogger _logger;
    private readonly StartupHostedServiceHealthCheck _startupHostedServiceHealthCheck;

    public StartupHostedService(ILogger<StartupHostedService> logger,
        StartupHostedServiceHealthCheck startupHostedServiceHealthCheck)
    {
        _logger = logger;
        _startupHostedServiceHealthCheck = startupHostedServiceHealthCheck;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Startup Background Service is starting.");

        // Simulate the effect of a long-running startup task.
        Task.Run(async () =>
        {
            await Task.Delay(_delaySeconds * 1000);

            _startupHostedServiceHealthCheck.StartupTaskCompleted = true;

            _logger.LogInformation("Startup Background Service has started.");
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Startup Background Service is stopping.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}
