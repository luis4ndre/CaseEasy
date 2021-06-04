using CaseEasy.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CaseEasy.Domain.Models
{
    public class Carteira
    {
        public Carteira()
        {
            this.Investimentos = new List<IInvestimento>();
        }

        public double ValorTotal => Investimentos.Sum(x => x.ValorTotal);

        public ICollection<IInvestimento> Investimentos { get; set; }

        public bool IsValid()
        {
            return this.Investimentos.Any();
        }
    }
}
