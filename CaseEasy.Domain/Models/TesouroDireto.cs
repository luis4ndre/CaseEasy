using CaseEasy.Domain.Extension;

namespace CaseEasy.Domain.Models
{
    public class TesouroDireto : InvestimentoBase
    {
        private const double _taxa = 10;
        public override double Ir => (this.ValorTotal - this.ValorInvestido).Percent(_taxa);
    }
}
