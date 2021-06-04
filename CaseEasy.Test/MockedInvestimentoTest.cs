using CaseEasy.API;
using CaseEasy.API.Services;
using CaseEasy.Domain.Interfaces;
using CaseEasy.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaseEasy.Test
{
    public class MockedInvestimentoTest
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly Mock<ILogger<InvestimentoService>> _mockLogger = new Mock<ILogger<InvestimentoService>>();

        private readonly IDictionary<string, Endpoint> _auxEndpoints = new Dictionary<string, Endpoint>()
        {
            {"Fundos", new Endpoint { Url = "http://www.mocky.io/v2/5e342ab33000008c00d96342", RootTag = "fundos" }},
            {"RendaFixa", new Endpoint { Url = "http://www.mocky.io/v2/5e3429a33000008c00d96336", RootTag = "lcis" }},
            {"TesouroDireto", new Endpoint { Url = "http://www.mocky.io/v2/5e3428203000006b00d9632a", RootTag = "tds" }}
        };


        public MockedInvestimentoTest()
        {
            this._appSettings = Options.Create(new AppSettings
            {
                CacheExpiration = 1,
                Fundos = this._auxEndpoints["Fundos"],
                RendaFixa = this._auxEndpoints["RendaFixa"],
                TesouroDireto = this._auxEndpoints["TesouroDireto"]
            });
        }

        [Fact]
        public async Task HappyPath()
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
               ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Equals(this._auxEndpoints["Fundos"].Url)),
               ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(GetResponse("Fundos"));

            mockHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
               ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Equals(this._auxEndpoints["RendaFixa"].Url)),
               ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(GetResponse("RendaFixa"));

            mockHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
               ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Equals(this._auxEndpoints["TesouroDireto"].Url)),
               ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(GetResponse("TesouroDireto"));

            var investimentoService = new InvestimentoService(this._appSettings, new MemoryCache(new MemoryCacheOptions()), new HttpClient(mockHandler.Object), this._mockLogger.Object);

            var carteira = await investimentoService.GetAllAsync();

            carteira.Should().NotBeNull();

            carteira.Investimentos.Should().NotBeEmpty();

            carteira.Investimentos.Should().HaveCount(6);

            carteira.ValorTotal.Should().BeApproximately(22399.597, 3);

            carteira.Investimentos.Sum(i => i.Ir).Should().BeApproximately(405.893, 3);

            carteira.Investimentos.Where(i => i is Fundo).Sum(i => i.Ir).Should().BeApproximately(368.928, 3);

            carteira.Investimentos.Where(i => i is Fundo).Sum(i => i.ValorResgate).Should().BeApproximately(11440.592, 3);

            carteira.Investimentos.Where(i => i is RendaFixa).Sum(i => i.Ir).Should().BeApproximately(30.381, 3);

            carteira.Investimentos.Where(i => i is RendaFixa).Sum(i => i.ValorResgate).Should().BeApproximately(6655.275, 3);

            carteira.Investimentos.Where(i => i is TesouroDireto).Sum(i => i.Ir).Should().BeApproximately(6.585, 3);

            carteira.Investimentos.Where(i => i is TesouroDireto).Sum(i => i.ValorResgate).Should().BeApproximately(1008.145, 3);
        }

        [Fact]
        public async Task WithoutFundos()
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
               ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Equals(this._auxEndpoints["Fundos"].Url)),
               ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(GetResponse("Error/Fundos"));

            mockHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
               ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Equals(this._auxEndpoints["RendaFixa"].Url)),
               ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(GetResponse("Error/RendaFixa"));

            mockHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
               ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Equals(this._auxEndpoints["TesouroDireto"].Url)),
               ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(GetResponse("Error/TesouroDireto"));

            var investimentoService = new InvestimentoService(this._appSettings, new MemoryCache(new MemoryCacheOptions()), new HttpClient(mockHandler.Object), this._mockLogger.Object);

            var carteira = await investimentoService.GetAllAsync();

            carteira.Should().NotBeNull();

            carteira.Investimentos.Should().NotBeEmpty();

            carteira.Investimentos.Should().HaveCount(4);

            carteira.Investimentos.Should().NotContain(i=>i is Fundo);
        }

        private HttpResponseMessage GetResponse(string tipo, string subFolder = "")
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(File.ReadAllText($"Responses/{subFolder}{tipo}.json"), Encoding.UTF8, "application/json")
            };
        }
    }
}
