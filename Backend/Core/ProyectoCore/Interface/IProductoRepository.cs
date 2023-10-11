using ProyectoCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface IProductoRepository
    {
        Task<ICollection<Producto>> GetProductosAsync();
        Task<Producto> GetProductosAsync(int id);
        Task<bool> UpdateProductoAsync(Producto producto);
        Task<List<int>> GetProductoIdsByPartialNamesAsync(List<string> partialNames);
        Task<ICollection<Producto>> GetProductosDescendingAsync();
        Task<List<Producto>> GetProductoCategoriaAsync();
        Task<List<Producto>> GetProductosByProductAndCategoryIdsAsync(List<int> productIds, List<int> categoryIds);
    }
}
