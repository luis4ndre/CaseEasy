using CaseEasy.Domain.Extension;
using System;
using System.Text.Json.Serialization;

namespace CaseEasy.Domain.Models
{
    public class Fundo : InvestimentoBase
    {
        private const double _taxa = 15;

        [JsonPropertyName("capitalInvestido")]
        public override double ValorInvestido { get; set; }

        [JsonPropertyName("ValorAtual")]
        public override double ValorTotal { get; set; }

        [JsonPropertyName("dataCompra")]
        public override DateTime DataDeCompra { get; set; }

        [JsonPropertyName("dataResgate")]
        public override DateTime Vencimento { get; set; }

        public override double Ir => (this.ValorTotal - this.ValorInvestido).Percent(_taxa);
    }
}
