using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CaseEasy.API.HealthCheck
{
    public class ExternalfHealthCheck
    {
        protected readonly AppSettings _appSettings;
        protected readonly HttpClient _httpClient;
        public ExternalfHealthCheck(IOptions<AppSettings> appSettings, HttpClient httpClient)
        {
            this._appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            this._httpClient = httpClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(string url)
        {
            var response = await this._httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
                return new HealthCheckResult(
                    HealthStatus.Healthy,
                    description: "Endpoint OK!");

            return new HealthCheckResult(
                    HealthStatus.Unhealthy,
                    description: $"Endpoint Down!");
        }
    }
}
