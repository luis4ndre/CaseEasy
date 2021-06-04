using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CaseEasy.API.HealthCheck
{
    public class FundosHealthCheck : ExternalfHealthCheck, IHealthCheck
    {
        public FundosHealthCheck(IOptions<AppSettings> appSettings, HttpClient httpClient) : base(appSettings, httpClient)
        {
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await base.CheckHealthAsync(base._appSettings.Fundos.Url);
        }
    }
}
