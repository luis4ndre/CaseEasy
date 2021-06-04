using CaseEasy.Domain.Interfaces;
using CaseEasy.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace CaseEasy.API.Services
{
    public class InvestimentoService : IInvestimentoService
    {
        private const string _chaveDoCache = "investimentos";
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<InvestimentoService> _logger;

        public InvestimentoService(IOptions<AppSettings> appSettings, IMemoryCache memoryCache, HttpClient httpClient, ILogger<InvestimentoService> logger)
        {
            this._appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            this._memoryCache = memoryCache;
            this._httpClient = httpClient;
            this._logger = logger;
        }

        public async Task<Carteira> GetAllAsync()
        {
            try
            {
                if (!this._memoryCache.TryGetValue(_chaveDoCache, out Carteira carteira) || carteira == null)
                {
                    carteira = new Carteira();

                    var investimentos = new List<IInvestimento>();

                    investimentos.AddRange(await ConsultaFundos());
                    investimentos.AddRange(await ConsultaRendaFixa());
                    investimentos.AddRange(await ConsultaTesouroDireto());

                    var opcoesDoCache = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddDays(this._appSettings.CacheExpiration)
                    };

                    carteira.Investimentos = investimentos;

                    this._memoryCache.Set(_chaveDoCache, carteira, opcoesDoCache);
                }

                if (!carteira.Investimentos.Any())
                    return null;

                return carteira;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);

                return null;
            }
        }

        private async Task<IEnumerable<TesouroDireto>> ConsultaTesouroDireto()
        {
            try
            {
                var response = await this._httpClient.GetAsync(this._appSettings.TesouroDireto.Url);

                if (response.IsSuccessStatusCode)
                    return await HandlerResponse<TesouroDireto>(response, this._appSettings.TesouroDireto.RootTag);

                this._logger.LogWarning($"ConsultaFundos {response.StatusCode}");

                return new List<TesouroDireto>();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);

                return new List<TesouroDireto>();
            }
        }

        private async Task<IEnumerable<RendaFixa>> ConsultaRendaFixa()
        {
            try
            {
                var response = await this._httpClient.GetAsync(this._appSettings.RendaFixa.Url);

                if (response.IsSuccessStatusCode)
                    return await HandlerResponse<RendaFixa>(response, this._appSettings.RendaFixa.RootTag);

                this._logger.LogWarning($"ConsultaFundos {response.StatusCode}");

                return new List<RendaFixa>();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);

                return new List<RendaFixa>();
            }
        }

        private async Task<IEnumerable<Fundo>> ConsultaFundos()
        {
            try
            {
                var response = await this._httpClient.GetAsync(this._appSettings.Fundos.Url);

                if (response.IsSuccessStatusCode)
                    return await HandlerResponse<Fundo>(response, this._appSettings.Fundos.RootTag);

                this._logger.LogWarning($"ConsultaFundos {response.StatusCode}");

                return new List<Fundo>();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);

                return new List<Fundo>();
            }
        }

        protected async Task<IEnumerable<T>> HandlerResponse<T>(HttpResponseMessage response, string rootTag) where T : IInvestimento
        {
            var investimentos = new List<T>();

            try
            {     
                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(content))
                    return null;

                if (JsonDocument.Parse(content).RootElement.TryGetProperty(rootTag, out var list))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    investimentos = JsonSerializer.Deserialize<List<T>>(list.ToString(), options);
                }

                return investimentos?.Where(i => i.IsValid());
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);

                return investimentos;
            }
        }
    }
}
