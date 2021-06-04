using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace CaseEasy.Test
{
    public class InvestimentoTest : IntegrationTestBase
    {
        [Fact]
        public async Task HappyPath()
        {
            var carteira = await base.InvestimentoService.GetAllAsync();

            carteira.Should().NotBeNull();

            carteira.Investimentos.Should().NotBeEmpty();
        }
    }
}
