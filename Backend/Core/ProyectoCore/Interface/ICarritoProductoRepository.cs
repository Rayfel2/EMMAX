using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface ICarritoProductoRepository
    {
        ICollection<CarritoProducto> GetCarritoProducto();
        Producto GetCarritoProducto(int id);
        bool save();
    }
}
