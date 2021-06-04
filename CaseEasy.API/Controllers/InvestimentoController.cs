using CaseEasy.Domain.Interfaces;
using CaseEasy.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DesafioEasy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestimentoController : ControllerBase
    {
        private readonly ILogger<InvestimentoController> _logger;

        private readonly IInvestimentoService _investimentoService;

        public InvestimentoController(IInvestimentoService investimentoService, ILogger<InvestimentoController> logger)
        {
            this._investimentoService = investimentoService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<CarteiraDTO> Get()
        {
            var carteira = await this._investimentoService.GetAllAsync();

            if (!carteira.IsValid())
            {
                this._logger.LogWarning("Nenhum investimento encontrado!");

                return null;
            }

            return new CarteiraDTO(carteira);
        }
    }
}
