using CaseEasy.Domain.Extension;
using System;
using System.Text.Json.Serialization;

namespace CaseEasy.Domain.Models
{
    public class RendaFixa : InvestimentoBase
    {
        private const double _taxa = 5;

        [JsonPropertyName("capitalInvestido")]
        public override double ValorInvestido { get; set; }

        [JsonPropertyName("capitalAtual")]
        public override double ValorTotal { get; set; }

        [JsonPropertyName("dataOperacao")]
        public override DateTime DataDeCompra { get; set; }

        public override double Ir => (this.ValorTotal - this.ValorInvestido).Percent(_taxa);
    }
}
