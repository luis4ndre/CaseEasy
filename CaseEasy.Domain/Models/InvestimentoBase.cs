using CaseEasy.Domain.Extension;
using CaseEasy.Domain.Interfaces;
using System;

namespace CaseEasy.Domain.Models
{
    public abstract class InvestimentoBase : IInvestimento
    {
        public string Nome { get; set; }
        public virtual double ValorInvestido { get; set; }
        public virtual double ValorTotal { get; set; }
        public virtual DateTime DataDeCompra { get; set; }
        public virtual DateTime Vencimento { get; set; }
        public abstract double Ir { get; }
        public double ValorResgate => ResgateCalc();

        public int DiasInvestidos => DateTime.Now.Subtract(this.DataDeCompra).Days;
        public int DiasCustodia => this.Vencimento.Subtract(this.DataDeCompra).Days;

        private double ResgateCalc()
        {
            if (this.DiasInvestidos > (this.DiasCustodia / 2))
                return this.ValorTotal.SubtractPercent(15);//15% de perda

            if (this.Vencimento.MonthDifference(DateTime.Now) <= 3)
                return this.ValorTotal.SubtractPercent(6);//6% de perda

            return this.ValorTotal.SubtractPercent(30);//30% de perda
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Nome) && this.ValorInvestido > 0 && this.DataDeCompra > DateTime.MinValue && this.Vencimento > DateTime.MinValue && this.Vencimento > this.DataDeCompra;
        }
    }
}
