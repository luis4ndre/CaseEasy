using CaseEasy.Domain.Models;
using System;
using System.Text.Json.Serialization;

namespace CaseEasy.Domain.Interfaces
{
    public interface IInvestimento
    {
        string Nome { get; set; }
        double ValorInvestido { get; set; }
        double ValorTotal { get; set; }
        double Ir { get; }
        double ValorResgate { get; }
        [JsonIgnore]
        DateTime DataDeCompra { get; set; }
        DateTime Vencimento { get; set; }
        bool IsValid();
    }
}
