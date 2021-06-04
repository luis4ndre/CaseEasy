using System.Collections.Generic;
using System.Linq;

namespace CaseEasy.Domain.Models
{
    public class CarteiraDTO
    {
        public CarteiraDTO(Carteira carteira)
        {
            this.ValorTotal = carteira.ValorTotal;
            this.Investimentos = carteira.Investimentos.Select(i => new InvestimentoDTO(i));
        }

        public double ValorTotal { get; set; }
        public IEnumerable<InvestimentoDTO> Investimentos { get; set; }
    }
}
