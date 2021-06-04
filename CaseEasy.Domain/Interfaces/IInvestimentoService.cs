using CaseEasy.Domain.Models;
using System.Threading.Tasks;

namespace CaseEasy.Domain.Interfaces
{
    public interface IInvestimentoService
    {
        Task<Carteira> GetAllAsync();
    }
}
