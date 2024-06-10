using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fcp.Jkr.Demo.Backend.1.Api.HealthChecks;
public class LivenessCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("OK"));
    }
}