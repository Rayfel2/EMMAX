using ProyectoCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface IMetodoRepository
    {
        Task<bool> SaveAsync();
        Task<ICollection<MetodoPago>> GetMetodoAsync();
        Task<MetodoPago> GetMetodoAsync(int id);
    }
}
