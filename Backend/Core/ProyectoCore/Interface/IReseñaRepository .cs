using ProyectoCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface IReseñaRepository
    {
        Task<ICollection<Reseña>> GetReseñasAsync();
        Task<ICollection<Reseña>> GetReseñasAsyncWithId(int id);
        Task<Reseña> GetReseñasAsync(int id);
        Task<bool> CreateReseñaAsync(Reseña reseña);
        Task<bool> SaveAsync();
    }
}
