using ProyectoCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface ICarritoRepository
    {
        Task<List<Carrito>> GetCarritoAsync();
        Task<Carrito> GetCarritoAsync(int id);
        Task<bool> CreateCarritoAsync(Carrito carrito);
        Task<bool> SaveAsync();
    }
}
