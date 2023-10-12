using ProyectoCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface ICarritoProductoRepository
    {
        Task<List<CarritoProducto>> GetCarritoProductoAsync();
        Task<List<CarritoProducto>> GetCarritoProductoAsync(int id);
        Task<bool> CreateCarritoProductoAsync(CarritoProducto carritoProducto);
        Task<bool> CarritoProductoExistAsync(int idCarrito, int idProducto);
        Task<bool> DeleteCarritoProductoAsync(CarritoProducto carritoProducto);
        Task<CarritoProducto> GetCarritosProductosAsync(int IdCarritos, int IdProductos);
        Task<bool> UpdateCarritoProductoAsync(CarritoProducto carritoProducto);
    }
}

