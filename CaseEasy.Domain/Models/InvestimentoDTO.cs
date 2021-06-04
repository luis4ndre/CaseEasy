using CaseEasy.Domain.Interfaces;
using System;

namespace CaseEasy.Domain.Models
{
    public class InvestimentoDTO
    {
        public InvestimentoDTO(IInvestimento investimento)
        {
            this.Nome = investimento.Nome;
            this.ValorInvestido = investimento.ValorInvestido;
            this.ValorTotal = investimento.ValorTotal;
            this.Ir = investimento.Ir;
            this.ValorResgate = investimento.ValorResgate;
            this.Vencimento = investimento.Vencimento;
        }

        public string Nome { get; set; }
        public double ValorInvestido { get; set; }
        public double ValorTotal { get; set; }
        public double Ir { get; }
        public double ValorResgate { get; }
        public DateTime Vencimento { get; set; }
    }
}
