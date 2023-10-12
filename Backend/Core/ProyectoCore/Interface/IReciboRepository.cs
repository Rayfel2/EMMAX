using ProyectoCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface IReciboRepository
    {
        Task<ICollection<Recibo>> GetReciboAsync();
        Task<Recibo> GetReciboAsync(int id);
        Task<bool> CreateReciboAsync(Recibo recibo);
        Task<bool> SaveAsync();
    }
}
