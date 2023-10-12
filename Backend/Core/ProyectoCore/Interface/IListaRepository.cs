using ProyectoCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface IListaRepository
    {
        Task<ListaDeseo> GetListaAsync(int id);
        Task<bool> CreateListaAsync(ListaDeseo lista);
        Task<ICollection<ListaDeseo>> GetListaAsync();
        Task<bool> SaveAsync();
    }
}
