using CaseEasy.API;
using CaseEasy.API.Services;
using CaseEasy.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;

namespace CaseEasy.Test
{
    public class IntegrationTestBase
    {
        private readonly IServiceScope _scope;
        protected readonly IInvestimentoService InvestimentoService;
        public IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddScoped<IInvestimentoService, InvestimentoService>();

                        services.AddMemoryCache();

                        services.AddSingleton(new HttpClient());

                    });
                });

            this._scope = appFactory.Services.CreateScope();

            this.InvestimentoService = this._scope.ServiceProvider.GetService<IInvestimentoService>();
        }
    }
}
